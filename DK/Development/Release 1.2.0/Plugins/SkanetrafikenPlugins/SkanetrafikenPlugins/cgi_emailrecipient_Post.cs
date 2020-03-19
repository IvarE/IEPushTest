//using system;
//using system.collections.generic;
//using system.linq;
//using system.text;

//using microsoft.xrm.sdk;
//using microsoft.xrm.sdk.query;
//using system.servicemodel;

//namespace crm2013.skanetrafikenplugins
//{
//    /*
//     * description:
//     * this plugin runs on cgi_emailrecipient in a post event. it checks if there is a contact
//     * with the same email adress, if not, it creates a contact.
//     */
//    public class cgi_emailrecipient_post : iplugin
//    {
//        private class plugindata : plugindatabase
//        {
//            public plugindata(iserviceprovider serviceprovider) : base(serviceprovider) { }
//        }

//        public void execute(iserviceprovider serviceprovider)
//        {
//            plugindata _data = new plugindata(serviceprovider);
            
//            try
//            {
//                if (_data.context.inputparameters.contains("target") && _data.context.inputparameters["target"] is entity)
//                {
//                    _data.target = (entity)_data.context.inputparameters["target"];
//                    if (_data.target.attributes.contains("cgi_emailaddress"))
//                    {
//                        if (_doescontactexist(_data) == false)
//                        {
//                            _createcontact(_data);
//                        }
//                    }
//                }
//            }
//            catch (faultexception<organizationservicefault> ex)
//            {
//                throw ex;
//            }
//            catch (exception ex)
//            {
//                throw ex;
//            }
//        }

//        private bool _doescontactexist(plugindata plugindata)
//        {
//            querybyattribute _query = new querybyattribute("contact");
//            _query.columnset = new columnset("emailaddress1"); // proberbly not needed as we only want to check count on return
//            _query.attributes.add("emailaddress1"); // email adress on contact
//            _query.values.add(plugindata.target.attributes["cgi_emailaddress"]); // email from entity emailrecipient
//            _query.attributes.add("statecode");
//            _query.values.add(0); // query only active
//            // if (records.entities == null || records.entities.count <= 0) return null;
//            entitycollection records = plugindata.service.retrievemultiple(_query);
//            return records.entities!=null && records.entities.count>0;
//        }

//        private void _createcontact(plugindata plugindata)
//        {
//            entity contact = new entity("contact");
//            contact.attributes["firstname"] = "ange";
//            contact.attributes["lastname"] = "namn";
//            contact.attributes["emailaddress1"] = plugindata.target.attributes["cgi_emailaddress"];
//            plugindata.service.create(contact);
//        }



//    }
//}
