using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;

namespace BG.Extensions
{
    /// <summary>
    /// Extensions for <see cref="System.ServiceModel.ICommunicationObject"/>.
    /// </summary>
    public static class ICommunicationObjectExtensions
    {
        public static void Using<TService>(this TService service, Action<TService> action, Func<TService> recallServiceCreator) where TService : ICommunicationObject
        {
            Using(service, action, recallServiceCreator, 0);
        }
        static void Using<TService>(this TService service, Action<TService> action, Func<TService> recallServiceCreator, int retries) where TService : ICommunicationObject
        {
            //Guard.Against<ArgumentNullException>(recallServiceCreator == null, "Ошибка вызова сервиса: не задана фабрика создания сервиса");

            try
            {
                //if (recallServiceCreator != null)
                //    throw new CommunicationException("Test");
                action(service);
            }
            //catch (FaultException<CrmServiceError> ex)
            //{
            //    throw;
            //}
            //catch (FaultException<AuthenticationCrmServiceError> ex)
            //{
            //    throw;
            //}
            catch (CommunicationException ex)
            {
                //Делаем еще одну попытку связанную с
                //http://stackoverflow.com/questions/1247185/there-was-no-endpoint-listening-at-net-pipe-localhost
                //I'm not sure whether this is germane to your discussion, because I've never used the .net named pipes,
                //but I do recall that the .net tcp socket endpoints had a known bug that resulted in "endpoints occasionally being terminated for no apparent reason",
                //and unfortunately the official MS response was a "workaround" which involved checking that the socket was still up before sending a message through it,
                //and re-opening it in the case that it wasn't. I'd like to think that the named pipe endpoints aren't as unreliable as the "reliable TCP endpoints",
                //but you might want to look into the "known periodic TCP socket failure" to see whether it also extends to named pipes.

                Thread.Sleep(500);

                if (retries < 3)
                {
                    retries++;
                    Using(recallServiceCreator(), action, recallServiceCreator, retries);
                }
                else
                    throw;

            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                if (service != null)
                    AsDisposable(service).Dispose();
            }
        }


        //////////////////////////////////////////////////
        ///http://caspershouse.com/post/Using-IDisposable-on-WCF-Proxies-(or-any-ICommunicationObject-implementation).aspx
        /// <author>Nicholas Paldino</author>
        /// <created>12/5/2009 2:21:11 PM</created>
        /// <summary>A structure which will take a <see cref="ICommunicationObject"/>
        /// instance and return an <see cref="IDisposable"/> implementation
        /// which will implement the close/abort pattern outlined here:
        /// http://msdn.microsoft.com/en-us/library/aa355056.aspx.</summary>
        /// <remarks>This structure enables the use of WCF clients (or any
        /// <see cref="ICommunicationObject"/> implementation really)
        /// in using statements.</remarks>
        ///
        //////////////////////////////////////////////////
        private struct DisposableCommunicationObjectToken : IDisposable
        {
            /// <summary>The <see cref="ICommunicationObject"/> that is to be closed/aborted
            /// of in the call to <see cref="Dispose"/>.</summary>
            private readonly ICommunicationObject client;

            //////////////////////////////////////////////////
            ///
            /// <author>Nicholas Paldino</author>
            /// <created>12/5/2009 2:21:11 PM</created>
            /// <summary>Creates an instance of the 
            /// <see cref="DisposableCommunicationObjectToken"/>.</summary>
            /// <param name="obj">The <see cref="ICommunicationObject"/>
            /// to apply the pattern to.</param>
            ///
            //////////////////////////////////////////////////
            internal DisposableCommunicationObjectToken(ICommunicationObject obj)
            {
                // The obj is not null.
                Guard.Against<ArgumentException>(obj == null,"Ошибка завершения прокси: не задан клиент прокси");

                // Store the obj.
                this.client = obj;
            }

            //////////////////////////////////////////////////
            ///
            /// <author>Nicholas Paldino</author>
            /// <created>12/5/2009 2:21:11 PM</created>
            /// <summary>Called when the instance is disposed.</summary>
            ///
            //////////////////////////////////////////////////
            public void Dispose()
            {
                // If the obj is null, throw an exception.
                if (client == null)
                {
                    // Throw the exception.
                    throw new InvalidOperationException(
                        "The DisposableCommunicationObjectToken structure " +
                        "was created with the default constructor.");
                }

                if (client.State == CommunicationState.Faulted || client.State == CommunicationState.Closed)
                {
                    client.Abort();
                    return;
                }

                // Try to close.
                try
                {
                    // Close.
                    client.Close();
                }
                catch (CommunicationException)
                {
                    // Abort if there is a communication exception.
                    client.Abort();
                }
                catch (TimeoutException)
                {
                    // Abort if there is a timeout exception.
                    client.Abort();
                }
                catch (Exception)
                {
                    // Abort in the face of any other exception.
                    client.Abort();

                    // Rethrow.
                    throw;
                }
            }
        }

        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>12/5/2009 2:21:11 PM</created>
        /// <summary>Takes an <see cref="ICommunicationObject"/>
        /// implementation and returns an <see cref="IDisposable"/>
        /// implementation which can be used in a using statement, while
        /// executing the proper cleanup procedure outlined here:
        /// http://msdn.microsoft.com/en-us/library/aa355056.aspx.</summary>
        /// <param name="obj">The <see cref="ICommunicationObject"/>
        /// implementation.</param>
        /// <returns>An <see cref="IDisposable"/> implementation which will
        /// close the <see cref="ICommunicationObject"/> implementation
        /// in <paramref name="obj"/> properly.</returns>
        ///
        //////////////////////////////////////////////////
        //
        // TODO: Add parameters here and on DisposableCommunicationObjectToken
        // TODO: of type Predicate<CommunicationException> and
        // TODO: Preidcate<TimeoutException> which will allow for
        // TODO: injected code which will allow for cleanup that the user specifies.
        // TODO: The return value from the predicate would determine if the exception
        // TODO: is rethrown.
        public static IDisposable AsDisposable(this ICommunicationObject obj)
        {
            // Return a new instance of the DisposableCommunicationObjectToken.
            return new DisposableCommunicationObjectToken(obj);
        }

        /// <summary>
        /// Closes the connection.
        /// </summary>
        /// <param name="serviceClient">The service client.</param>
        /// <remarks>
        /// For more detail see �IDisposable and WCF�
        /// by Steve Smith
        /// [http://stevesmithblog.com/blog/idisposable-and-wcf/]
        /// </remarks>
        public static void CloseConnection(this ICommunicationObject serviceClient)
        {
            if (serviceClient == null) return;
            if (serviceClient.State != CommunicationState.Opened) return;

            try
            {
                serviceClient.Close();
            }
            catch (CommunicationException ex)
            {
                serviceClient.Abort();
            }
            catch (TimeoutException ex)
            {
                serviceClient.Abort();
            }
            catch (Exception ex)
            {
                serviceClient.Abort();
                throw;
            }
        }

        /// <summary>
        /// Closes the connection or abort.
        /// </summary>
        /// <param name="serviceClient">The service client.</param>
        /// <param name="waitInMilliseconds">The wait in milliseconds.</param>
        public static void CloseConnectionOrAbort(this ICommunicationObject serviceClient, int waitInMilliseconds = 500)
        {
            if (serviceClient == null) return;
            serviceClient.CloseConnection();

            Thread.Sleep(waitInMilliseconds);

            if ((serviceClient.State != CommunicationState.Closed) &&
                (serviceClient.State != CommunicationState.Closing))
            {
                serviceClient.Abort();
            }
        }

        /// <summary>
        /// Determines whether the specified service client is opened.
        /// </summary>
        /// <param name="serviceClient">The service client.</param>
        /// <returns>
        ///   <c>true</c> if the specified service client is opened; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOpened(this ICommunicationObject serviceClient)
        {
            return (serviceClient.State == CommunicationState.Opened);
        }
    }
}


