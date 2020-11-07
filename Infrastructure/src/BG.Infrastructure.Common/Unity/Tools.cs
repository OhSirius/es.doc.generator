using System;
using System.Collections.Generic;
using System.Management;
using System.Text;
using System.IO;
using System.IO.Compression;

using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace BG.Infrastructure.Common
{
    #region class ImageTools	- Вспомогательные средства
    public class ImageTools
    {
        public const int Jpeg_Default_Image_HighQuality = 100;
        public const int Jpeg_Default_Image_Quality = 80;
        public const int Jpeg_Default_ImagePreview_Quality = 15;


        #region .ScaleImage
        /// <summary>
        /// ScaleImage
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        public static Image ScaleImage(int width, int height, Image image)
        {
            return ScaleImage(width, height, image,Color.White);
        }
        public static Image ScaleImage(int width, int height, Image image, Color back_color)
        {
            if (image == null)
                return null;
            try
            {
                Image nimg = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                Graphics gr = Graphics.FromImage(nimg);
                gr.Clear(back_color);
                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;


                int w = image.Size.Width;
                int h = image.Size.Height;

                if (image.Size.Width > image.Size.Height)
                {
                    w = width;
                    //h=(height*image.Size.Height)/image.Size.Width;
                    h = (w * image.Size.Height) / image.Size.Width;
                }
                else
                {
                    h = height;
                    //w=(width*image.Size.Width)/image.Size.Height;
                    w = (h * image.Size.Width) / image.Size.Height;
                }

                gr.DrawImage(image, new Rectangle((width - w) / 2, (height - h) / 2, w, h), new Rectangle(0, 0, image.Size.Width, image.Size.Height), GraphicsUnit.Pixel);

                gr.Dispose();

                return nimg;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region .ScaleImageWithProportion
        /// <summary>
        /// ScaleImage
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        public static Image ScaleImageWithProportion(int width, int height, Image image)
        {
            if (image == null)
                return null;
            try
            {
                int w = image.Size.Width;
                int h = image.Size.Height;

                if (image.Size.Width > image.Size.Height)
                {
                    w = width;
                    //h=(height*image.Size.Height)/image.Size.Width;
                    h = (w * image.Size.Height) / image.Size.Width;
                    if (h > height)
                    {
                        w = (w * height) / h;
                        h = height;
                    }
                }
                else
                {
                    h = height;
                    //w=(width*image.Size.Width)/image.Size.Height;
                    w = (h * image.Size.Width) / image.Size.Height;

                    if (w > width)
                    {
                        h = (h * width) / w;
                        w = width;
                    }
                }
                Image nimg = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                Graphics gr = Graphics.FromImage(nimg);
                gr.Clear(Color.White);
                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

                gr.DrawImage(image, new Rectangle(0, 0, w, h), new Rectangle(0, 0, image.Size.Width, image.Size.Height), GraphicsUnit.Pixel);

                gr.Dispose();

                return nimg;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region .ScaleImageWithProportion
        /// <summary>
        /// ScaleImage
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        public static Image ScaleImageWithProportionOld(int width, int height, Image image)
        {
            if (image == null)
                return null;
            try
            {
                int w = image.Size.Width;
                int h = image.Size.Height;

                if (image.Size.Width > image.Size.Height)
                {
                    w = width;
                    //h=(height*image.Size.Height)/image.Size.Width;
                    h = (w * image.Size.Height) / image.Size.Width;
                }
                else
                {
                    h = height;
                    //w=(width*image.Size.Width)/image.Size.Height;
                    w = (h * image.Size.Width) / image.Size.Height;
                }
                Image nimg = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                Graphics gr = Graphics.FromImage(nimg);
                gr.Clear(Color.White);
                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

                gr.DrawImage(image, new Rectangle(0, 0, w, h), new Rectangle(0, 0, image.Size.Width, image.Size.Height), GraphicsUnit.Pixel);

                gr.Dispose();

                return nimg;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region .GetImageBytesWithJpegQuality

        static EncoderParameters myEncoderParameters;
        static ImageCodecInfo myImageCodecInfo;

        /// <summary>
        /// GetImageBytesWithJpegQuality
        /// </summary>
        /// <param name="image"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static byte[] GetImageBytesWithJpegQuality(Image image, int level)
        {
            if (image == null)
                return null;
            byte[] buff = null;

            try
            {
                if (myEncoderParameters == null)
                {
                    myImageCodecInfo = null;
                    ImageCodecInfo[] en_list = ImageCodecInfo.GetImageEncoders();
                    foreach (ImageCodecInfo en in en_list)
                    {
                        if (en.MimeType.ToLower() == "image/jpeg")
                        {
                            myImageCodecInfo = en;
                            break;
                        }
                    }

                    System.Drawing.Imaging.Encoder mQuality = System.Drawing.Imaging.Encoder.Quality;
                    EncoderParameter myEncoderParameter = new EncoderParameter(mQuality, (long)level);//25L); 

                    myEncoderParameters = new EncoderParameters(1);
                    myEncoderParameters.Param[0] = myEncoderParameter;
                }

                MemoryStream mem_stream = new MemoryStream();
                image.Save(mem_stream, myImageCodecInfo, myEncoderParameters);
                buff = mem_stream.ToArray();
                mem_stream.Close();

                return buff;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }

            try
            {
                MemoryStream mem_stream = new MemoryStream();
                image.Save(mem_stream, ImageFormat.Jpeg);
                buff = mem_stream.ToArray();
                mem_stream.Close();

                return buff;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
            try
            {
                MemoryStream mem_stream = new MemoryStream();
                image.Save(mem_stream, ImageFormat.Gif);
                buff = mem_stream.ToArray();
                mem_stream.Close();

                return buff;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
            try
            {
                MemoryStream mem_stream = new MemoryStream();
                image.Save(mem_stream, ImageFormat.Bmp);
                buff = mem_stream.ToArray();
                mem_stream.Close();

                return buff;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
            return null;

        }
        #endregion


 
        #region .GetImageWithOutAplha
        /// <summary>
        /// GetImageWithOutAplha
        /// </summary>
        /// <param name="image"></param>
        /// <param name="back_color"></param>
        /// <returns></returns>
        public static Image GetImageWithOutAplha(Image image, Color back_color)
        {
            if (image == null)
                return null;
            try
            {
                Image nimg = new Bitmap(image.Size.Width, image.Size.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                Graphics gr = Graphics.FromImage(nimg);
                gr.Clear(back_color);
                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                gr.DrawImage(image, new Rectangle(0, 0, image.Size.Width, image.Size.Height), new Rectangle(0, 0, image.Size.Width, image.Size.Height), GraphicsUnit.Pixel);
                gr.Dispose();

                return nimg;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region .GetVerticalGradientImage
        /// <summary>
        /// GetVerticalGradientImage
        /// </summary>
        /// <param name="height"></param>
        /// <param name="top_color"></param>
        /// <param name="bottom_color"></param>
        /// <returns></returns>
        public static Image GetVerticalGradientImage(int height, Color top_color, Color bottom_color)
        {
            if (height <= 0)
                return null;
            try
            {
                Image nimg = new Bitmap(1, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                Graphics gr = Graphics.FromImage(nimg);
                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;


                using (System.Drawing.Drawing2D.LinearGradientBrush br = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, 1, height),
                          top_color,
                          bottom_color,
                          System.Drawing.Drawing2D.LinearGradientMode.Vertical))
                {
                    gr.FillRectangle(br, 0, 0, 1, height);
                }

                gr.Dispose();

                return nimg;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region GetImageBytes
        /// <summary>
        /// GetImageBytes
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static byte[] GetImageBytes(Image img)
        {
            try
            {
                MemoryStream mem_stream = new MemoryStream();
                img.Save(mem_stream, System.Drawing.Imaging.ImageFormat.Bmp);

                byte[] buff = new byte[mem_stream.Length];
                mem_stream.Seek(0, SeekOrigin.Begin);
                mem_stream.Read(buff, 0, (int)mem_stream.Length);
                mem_stream.Close();

                return buff;
            }
            catch { }

            return null;
        }
        #endregion

        #region GetPngImageBytes
        /// <summary>
        /// GetImageBytes
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static byte[] GetPngImageBytes(Image img)
        {
            try
            {
                MemoryStream mem_stream = new MemoryStream();
                img.Save(mem_stream, System.Drawing.Imaging.ImageFormat.Png);

                byte[] buff = new byte[mem_stream.Length];
                mem_stream.Seek(0, SeekOrigin.Begin);
                mem_stream.Read(buff, 0, (int)mem_stream.Length);
                mem_stream.Close();

                return buff;
            }
            catch { }

            return null;
        }
        #endregion

        #region GetImageByBytes
        /// <summary>
        /// GetImageBytes
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static Image GetImageByBytes(byte[] buff)
        {
            try
            {
                System.IO.MemoryStream rd = new System.IO.MemoryStream(buff);
                Image img = Image.FromStream(rd);
                rd.Close();

                return img;
            }
            catch { }

            return null;
        }
        #endregion

        #region GetImageBytesAsBmp
        /// <summary>
        /// GetImageBytesAsBmp
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static byte[] GetImageBytesAsBmp(Image img)
        {
            if (img == null)
                return null;
            try
            {
                MemoryStream mem_stream = new MemoryStream();

                Image img_d = ScaleImage(img.Width, img.Height, img);

                img_d.Save(mem_stream, System.Drawing.Imaging.ImageFormat.Bmp);

                byte[] buff = new byte[mem_stream.Length];
                mem_stream.Seek(0, SeekOrigin.Begin);
                mem_stream.Read(buff, 0, (int)mem_stream.Length);
                mem_stream.Close();

                return buff;
            }
            catch { }

            return null;
        }
        #endregion

        #region CovertToRTF
        /// <summary>
        /// CovertToRTF
        /// </summary>
        /// <param name="img"></param>
        /// <returns>
        /// {\pict\picwgoal240\pichgoal240\wmetafile8 01000900..ff030000000000}
        /// </returns>
        public static string CovertToRTF(Image img)
        {
            try
            {
                byte[] buff = GetImageBytesAsBmp(img);
                int w = (240 / 16) * img.Width;
                int h = (240 / 16) * img.Height;
                string s = @"{\pict\picwgoal" + w.ToString() + @"\pichgoal" + h.ToString() + @"\wmetafile8 " + ToHexString(buff) + "}";

                return s;
            }
            catch { }

            return null;
        }
        #endregion

        #region ToHexString
        static char[] hexDigits = {
									  '0', '1', '2', '3', '4', '5', '6', '7',
									  '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'};

        public static string ToHexString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length * 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                int b = bytes[i];
                chars[i * 2] = hexDigits[b >> 4];
                chars[i * 2 + 1] = hexDigits[b & 0xF];
            }
            return new string(chars);
        }
        #endregion

    }
    #endregion


    #region class ControlUpdate	- Вспомогательные средства
    public class ControlUpdate : IDisposable
    {
        static object lock_obj = new object();

        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public static extern bool IsWindow(IntPtr hwnd);

        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public static extern int SendMessage(
            IntPtr hWnd,
            uint Msg,
            int wParam,
            int lParam
            );
        public const int WM_SETREDRAW = 0x000B;



        #region Reset()
        public static void Reset()
        {
            lock (lock_obj)
            {
                table.Clear();
            }
            //Aetp.Windows.Forms.Win32.LockWindowUpdate(null);
        }
        #endregion

        #region Lock(Control control)
        public static ControlUpdate Lock(Control control)
        {
            return new ControlUpdate(control);
            return null;// new ControlUpdate(control);
        }
        #endregion

        #region ControlUpdateLock(Control control)

        static System.Collections.Hashtable table = new System.Collections.Hashtable();

        Control _control = null;

        private ControlUpdate()
        {
        }
        private ControlUpdate(Control control)
        {
            _control = control;
            if (control == null)
                return;

            lock (lock_obj)
            {
                if (table.Contains(control))
                {
                    table[control] = ((int)table[control]) + 1;
                }
                else
                {
                    table.Add(control, 1);

                   // if (IsWindow(control.Handle))
                     //   SendMessage(control.Handle, WM_SETREDRAW, 0, 0);
                //Закомментировал    
                //Aetp.Windows.Forms.Win32.LockWindowUpdate(control);
                }
            }
        }
        ~ControlUpdate()
        {
            Dispose();
        }
        #endregion

        #region Dispose()
        public void Dispose()
        {
            lock (lock_obj)
            {
                if (_control == null)
                    return;
                try
                {
                    if (table.Contains(_control))
                    {
                        int val = ((int)table[_control]) - 1;
                        table[_control] = val;
                        if (val > 0)
                            return;

                        table.Remove(_control);
                    }
                }
                catch { }

                //if (IsWindow(_control.Handle))
                {
                  //  SendMessage(_control.Handle, WM_SETREDRAW, 1, 0);
                  //  _control.Refresh();
                    //_control.Invalidate(true);
                }

                //Убрал, т.к. реализация в CRM
                //Aetp.Windows.Forms.Win32.LockWindowUpdate(null);
                _control = null;
            }

            GC.SuppressFinalize(this);
        }
        #endregion
    }
    #endregion

    #region class Tools	- Вспомогательные средства
    /// <summary>
    /// Вспомогательные средства
    /// </summary>
    public class Tools
    {
        public static string GetTextForSearch(string text)
        {
            return string.IsNullOrEmpty(text) == false && text.Length > 7998 ? text.Substring(0, 7998).Trim() : text.Trim();
        }

        #region >GetImage
        /// <summary>
        /// Загрузить картинку
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal static Image GetImage(string name)
        {
            try
            {
                System.IO.Stream stream = null;
                System.Drawing.Image img = null;

                stream = System.Reflection.Assembly.GetCallingAssembly().GetManifestResourceStream(name);
                if (stream != null)
                    img = Image.FromStream(stream);
                return img;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region > GetImageCached - Загрузить картинку

        static System.Collections.Hashtable table_images = null;
        /// <summary>
        /// Загрузить картинку
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal static Image GetImageCached(string name)
        {
            try
            {
                if (table_images == null)
                {
                    table_images = new System.Collections.Hashtable();

                }

                if (table_images.Contains(name) == true)
                {
                    return table_images[name] as Image;
                }

                System.IO.Stream stream = null;
                System.Drawing.Image img = null;

                stream = System.Reflection.Assembly.GetCallingAssembly().GetManifestResourceStream(name);
                if (stream != null)
                    img = Image.FromStream(stream);

                if (img != null)
                    table_images.Add(name, img);

                return img;
            }
            catch
            {
                return null;
            }
        }

        #endregion
        #region > GetResourceBytes - Загрузить объект

        /// <summary>
        /// Загрузить объект
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal static byte[] GetResourceBytes(string name)
        {
            try
            {
                System.IO.Stream stream = null;

                stream = typeof(Tools).Assembly.GetManifestResourceStream(name);
                if (stream != null)
                {
                    long len = stream.Length;
                    if (len > 0)
                    {
                        byte[] buff = new byte[len];
                        stream.Read(buff, 0, (int)len);
                        return buff;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        #endregion
        #region > GetResourceString - Загрузить объект

        /// <summary>
        /// Загрузить объект
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal static string GetResourceString(string name)
        {
            try
            {
                System.IO.Stream stream = null;

                stream = typeof(Tools).Assembly.GetManifestResourceStream(name);
                if (stream != null)
                {
                    long len = stream.Length;
                    if (len > 0)
                    {
                        //byte[] buff=new byte[len];
                        //stream.Read(buff,0,(int)len);
                        StreamReader rd = new StreamReader(stream);
                        string s = rd.ReadToEnd();
                        rd.Close();

                        return s;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region .ScaleImage
        /// <summary>
        /// ScaleImage
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        public static Image ScaleImage(int width, int height, Image image)
        {
            if (image == null)
                return null;
            try
            {
                Image nimg = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                Graphics gr = Graphics.FromImage(nimg);
                gr.Clear(Color.White);
                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

                int w = image.Size.Width;
                int h = image.Size.Height;

                if (image.Size.Width > image.Size.Height)
                {
                    w = width;
                    //h=(height*image.Size.Height)/image.Size.Width;
                    h = (w * image.Size.Height) / image.Size.Width;
                }
                else
                {
                    h = height;
                    //w=(width*image.Size.Width)/image.Size.Height;
                    w = (h * image.Size.Width) / image.Size.Height;
                }

                gr.DrawImage(image, new Rectangle((width - w) / 2, (height - h) / 2, w, h), new Rectangle(0, 0, image.Size.Width, image.Size.Height), GraphicsUnit.Pixel);

                gr.Dispose();

                return nimg;
            }
            catch
            {
                return null;
            }
        }
        #endregion



        #region >ColorLight,ColorDark,ColorModif
        /// <summary>
        /// ColorLight
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        static public Color ColorLight(Color c)
        {
            return ColorModif(c, 25);
        }
        /// <summary>
        /// ColorDark
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        static public Color ColorDark(Color c)
        {
            return ColorModif(c, -25);
        }


        /// <summary>
        /// ColorModif
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        static public Color ColorModif(Color c, int depth)
        {
            int r = c.R + depth;
            int g = c.G + depth;
            int b = c.B + depth;

            r = r < 0 ? 0 : r;
            g = g < 0 ? 0 : g;
            b = b < 0 ? 0 : b;

            r = r > 255 ? 255 : r;
            g = g > 255 ? 255 : g;
            b = b > 255 ? 255 : b;

            return Color.FromArgb(r, g, b);
        }
        /// <summary>
        /// ColorInvert
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        static public Color ColorInvert(Color c)
        {
            int r = c.R;
            int g = c.G;
            int b = c.B;

            r = 255 - r;
            g = 255 - g;
            b = 255 - b;


            r = r < 0 ? 0 : r;
            g = g < 0 ? 0 : g;
            b = b < 0 ? 0 : b;

            r = r > 255 ? 255 : r;
            g = g > 255 ? 255 : g;
            b = b > 255 ? 255 : b;

            return Color.FromArgb(r, g, b);
        }
        #endregion


        #region >GetTimeForReport  - "YYYY-MM-DD HH-MM-SS"
        /// <summary>
        /// Получить текущее время в формате "YYYY-MM-DD HH-MM-SS"
        /// </summary>
        /// <returns></returns>
        public static string GetTimeForReport()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"); ;
        }
        #endregion

        #region >InternString
        /// <summary>
        /// InternString
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string InternString(string str)
        {
            if (str == null)
                return str;
            return string.Intern(str);
        }
        #endregion

        #region ToHexString
        static char[] hexDigits = {
									  '0', '1', '2', '3', '4', '5', '6', '7',
									  '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'};

        public static string ToHexString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length * 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                int b = bytes[i];
                chars[i * 2] = hexDigits[b >> 4];
                chars[i * 2 + 1] = hexDigits[b & 0xF];
            }
            return new string(chars);
        }
        #endregion

        #region StrCat
        public static string StrCat(string s, int max_len)
        {
            if (string.IsNullOrEmpty(s) || s.Length <= max_len)
                return s;

            if (max_len < 6)
                return s.Substring(0, max_len);

            return s.Substring(0, max_len - 3) + "...";
        }
        #endregion
        //

        #region >GetMoneyString - {0:### ### ### ### ##0.##}
        public static string GetMoneyString(object value)
        {
            return string.Format("{0:### ### ### ### ##0.##}", value).Trim();
        }
        #endregion

        #region >GetMoneyStringForReport - {0:### ### ### ### ##0.##}
        public static string GetMoneyStringForReport(object value)
        {
            return GetMoneyString(value).Replace(" ", "&nbsp;");
        }
        #endregion

        #region TimeTToLong
        static private long TimeTToLong(uint t)
        {
            return ((long)t) * (long)10000000 + (long)116444736000000000;
        }
        #endregion

        #region LongToTimeT
        static private uint LongToTimeT(long l)
        {
            return (uint)((l - (long)116444736000000000) / ((long)10000000));
        }
        #endregion

        #region TimeTToDateTime
        static public DateTime TimeTToDateTime(uint t)
        {
            return DateTime.FromFileTime(TimeTToLong(t));
        }
        #endregion

        #region DateTimeToTimeT
        static public uint DateTimeToTimeT(DateTime t)
        {
            return LongToTimeT(t.ToFileTime());
        }
        #endregion

        //
        public static string Tr(string s)
        {
            return s;
        }
        public static string Tr(Control c)
        {
            return "";
        }
        public static string Tr(Control c,string s)
        {
            return s;
        }

        #region Rand

        private static Random rnd = new Random((int)DateTime.Now.Ticks);

        /// <summary>
        /// Rand
        /// </summary>
        /// <returns></returns>
        public static int Rand()
        {
            return rnd.Next();
        }
        #endregion


        #region >GetValueFromSprList
        public static string GetValueFromSprList(string str_list, string param_name)
        {
            return GetValueFromSprList(str_list, param_name, '\n');
        }
        /// <summary>
        /// GetValueFromSprList
        /// </summary>
        /// <param name="str_list">"ValueName=Value"</param>
        /// <param name="param_name"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string GetValueFromSprList(string str_list, string param_name, string separator)
        {
            if (FastString.IsEmpty(str_list) || FastString.IsEmpty(param_name))
                return "";
            int index_begin;

            param_name += "=";

            int param_name_Length = param_name.Length;

            if ((index_begin = FastString.IndexOf(str_list, separator + param_name)) >= 0
                || (index_begin = FastString.IndexOf(str_list, param_name)) == 0)
            {
                int index_end;
                if (index_begin == 0)
                    index_begin += param_name_Length;
                else
                    index_begin += separator.Length + param_name_Length;

                index_end = FastString.IndexOf(str_list, separator, index_begin);
                if (index_end > 0)
                    return str_list.Substring(index_begin, index_end - index_begin);
                else
                    return str_list.Substring(index_begin);
            }
            return "";
        }
        /// <summary>
        /// GetValueFromSprList
        /// </summary>
        /// <param name="str_list">"ValueName=Value"</param>
        /// <param name="param_name"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string GetValueFromSprList(string str_list, string param_name, char separator)
        {
            if (FastString.IsEmpty(str_list) || FastString.IsEmpty(param_name))
                return "";

            int index_begin;

            param_name += "=";

            int param_name_Length = param_name.Length;

            if ((index_begin = FastString.IndexOf(str_list, separator + param_name)) >= 0
                || (index_begin = FastString.IndexOf(str_list, param_name)) == 0)
            {
                if (index_begin == 0)
                    index_begin += param_name_Length;
                else
                    index_begin += 1 + param_name_Length;

                int index_end;

                index_end = str_list.IndexOf(separator, index_begin);
                if (index_end > 0)
                    return str_list.Substring(index_begin, index_end - index_begin);
                else
                    return str_list.Substring(index_begin);
            }
            return "";
        }
        #endregion

        #region >SetValueToSprList
        public static string SetValueToSprList(string str_list, string param_name, string value)
        {
            return SetValueToSprList(str_list, param_name, value, '\n');
        }
        /// <summary>
        /// SetValueToSprList
        /// </summary>
        /// <param name="str_list">"ValueName=Value"</param>
        /// <param name="param_name"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string SetValueToSprList(string str_list, string param_name, string value, string separator)
        {
            int index_begin;

            param_name += "=";

            int param_name_Length = param_name.Length;

            if ((index_begin = FastString.IndexOf(str_list, separator + param_name)) >= 0
                || (index_begin = FastString.IndexOf(str_list, param_name)) == 0)
            {
                int index_end;
                if (index_begin == 0)
                    index_begin += param_name_Length;
                else
                    index_begin += separator.Length + param_name_Length;

                index_end = FastString.IndexOf(str_list, separator, index_begin);
                if (index_end > 0)
                {
                    //str_list=str_list.Substring(index_begin,index_end-index_begin);
                    str_list = str_list.Remove(index_begin, index_end - index_begin);
                    str_list = str_list.Insert(index_begin, value);

                    return str_list;
                }
                else
                {
                    //return str_list.Substring(index_begin);
                    str_list = str_list.Remove(index_begin, str_list.Length - index_begin);
                    str_list = str_list.Insert(index_begin, value);

                    return str_list;
                }
            }
            if (str_list.Length > 0)
                if (str_list[str_list.Length - 1].ToString() != separator)
                    str_list += separator.ToString();

            str_list = str_list + param_name +  value + separator;

            return str_list;
        }
        /// <summary>
        /// SetValueToSprList
        /// </summary>
        /// <param name="str_list">"ValueName=Value"</param>
        /// <param name="param_name"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string SetValueToSprList(string str_list, string param_name, string value, char separator)
        {
            int index_begin;

            param_name += "=";

            int param_name_Length = param_name.Length;

            if ((index_begin = FastString.IndexOf(str_list, separator + param_name)) >= 0
                || (index_begin = FastString.IndexOf(str_list, param_name)) == 0)
            {
                if (index_begin == 0)
                    index_begin += param_name_Length;
                else
                    index_begin += 1 + param_name_Length;

                int index_end;

                index_end = str_list.IndexOf(separator, index_begin);
                if (index_end > 0)
                {
                    str_list = str_list.Remove(index_begin, index_end - index_begin);
                    str_list = str_list.Insert(index_begin, value);

                    return str_list;
                }
                else
                {
                    str_list = str_list.Remove(index_begin, str_list.Length - index_begin);
                    str_list = str_list.Insert(index_begin, value);

                    return str_list;
                }
            }
            if (str_list.Length > 0)
                if (str_list[str_list.Length - 1] != separator)
                    str_list += separator;

            str_list = str_list + param_name + value + separator;

            return str_list;
        }
        #endregion

        #region GetHardwareId
        public static string GetHardwareId()
        {
            //string hash = "";
            //ManagementObjectSearcher searcher = null;
            //try
            //{
            //    string hardwareId = "";

            //    //процессор
            //    searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
            //    foreach (ManagementObject queryObj in searcher.Get())
            //    {
            //        hardwareId = queryObj["ProcessorId"].ToString();
            //        break;
            //    }

            //    //мать
            //    searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM CIM_Card");

            //    foreach (ManagementObject queryObj in searcher.Get())
            //    {
            //        hardwareId = string.Format("{0} {1} {2}", hardwareId, queryObj["Manufacturer"].ToString().Trim(),
            //                                   queryObj["Product"].ToString().Trim());
            //        break;
            //    }
            //    if (hardwareId != "")
            //        hash = FastString.GetStringHashAsString(hardwareId);
            //    searcher.Dispose();
            //}
            //catch { }
            //finally
            //{
            //    if (searcher != null)
            //        searcher.Dispose();
            //}
            //return hash;

            string hardwareId = "unknown";
            try
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor"))
                {
                    foreach (var o in searcher.Get())
                    {
                        var queryObj = (ManagementObject)o;
                        hardwareId = queryObj["ProcessorId"].ToString();
                    }
                }
            }
            catch
            {
                return "Exception";
            }
            return hardwareId;
        } 
        #endregion

        #region MapWebPath
        public static string MapWebPath(string web_path = null)
        {
            string site = System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath;
            site = site != null ? site.Trim('/') : "";
            if (!string.IsNullOrEmpty(site))
                site = string.Format(@"\{0}", site);

            if (!string.IsNullOrEmpty(web_path))
            {
                web_path = web_path.Replace(@"\", @"/");

                if (!web_path.StartsWith("/"))
                    web_path = string.Format("/{0}", web_path);
            }

            string res = string.Format(@"{0}{1}", site, web_path);
            if (string.IsNullOrEmpty(res))
                res = "/";
            else if (!res.StartsWith("/"))
                res = string.Format("/{0}", res);

            return System.Web.Hosting.HostingEnvironment.MapPath(res);
        }
        #endregion
    }
    #endregion


    #region class FileTools
    public class FileTools
    {
        #region FileForDelete
        class FileForDelete
        {
            public string file_name;

            public FileForDelete(string name)
            {
                file_name = name;
            }
            ~FileForDelete()
            {
                try
                {
                    System.IO.File.Delete(file_name);
                }
                catch { }
            }
        }
        static List<FileForDelete> list = new List<FileForDelete>();
        #endregion


        #region AddForDelete
        static public void AddForDelete(string file)
        {
            list.Add(new FileForDelete(file));
        }
        #endregion

        #region DeleteFiles
        static public void DeleteFiles()
        {
            foreach (FileForDelete file in list)
            {
                try
                {
                    System.IO.File.Delete(file.file_name);
                }
                catch { }
            }
            list.Clear();
        }
        #endregion
    }
    #endregion

}
