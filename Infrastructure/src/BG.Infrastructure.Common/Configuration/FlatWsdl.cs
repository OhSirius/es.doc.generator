using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Web.Services.Description;
using ServiceDescription = System.Web.Services.Description.ServiceDescription;


namespace BG.Infrastructure.Common.Configuration
{
    public class FlatWsdl : IWsdlExportExtension, IEndpointBehavior
    {
        public void ExportContract(WsdlExporter exporter, WsdlContractConversionContext context)
        {
        }


        public void ExportEndpoint(WsdlExporter exporter, WsdlEndpointConversionContext context)
        {
            XmlSchemaSet schemaSet = exporter.GeneratedXmlSchemas;

            foreach (ServiceDescription wsdl in exporter.GeneratedWsdlDocuments)
            {

                var importsList = new List<XmlSchema>();

                foreach (XmlSchema schema in wsdl.Types.Schemas)
                {
                    AddImportedSchemas(schema, schemaSet, importsList);
                }

                wsdl.Types.Schemas.Clear();

                foreach (XmlSchema schema in importsList)
                {
                    RemoveXsdImports(schema);
                    wsdl.Types.Schemas.Add(schema);
                }
            }
        }

        private void AddImportedSchemas(XmlSchema schema, XmlSchemaSet schemaSet, List<XmlSchema> importsList)
        {
            foreach (XmlSchemaImport import in schema.Includes)
            {
                ICollection realSchemas =
                    schemaSet.Schemas(import.Namespace);

                foreach (XmlSchema ixsd in realSchemas)
                {
                    if (!importsList.Contains(ixsd))
                    {
                        importsList.Add(ixsd);
                        AddImportedSchemas(ixsd, schemaSet, importsList);
                    }
                }
            }
        }

        private void RemoveXsdImports(XmlSchema schema)
        {
            for (int i = 0; i < schema.Includes.Count; i++)
            {
                if (schema.Includes[i] is XmlSchemaImport)
                    schema.Includes.RemoveAt(i--);
            }
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }
}
