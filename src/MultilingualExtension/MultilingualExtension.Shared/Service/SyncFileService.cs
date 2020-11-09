using System;
using System.Xml;
using MultilingualExtension.Shared.Helpers;

namespace MultilingualExtension.Shared.Service
{
    public class SyncFileService
    {
        public SyncFileService()
        {
        }
        public Shared.Helpers.Result<Boolean> SyncFile(string masterfilePath, string updatefilePath, bool addMasterCommentNode, Shared.Interface.IProgressBar progress)
        {



            XmlDocument masterdoc = new XmlDocument();
            masterdoc.Load(masterfilePath);
            XmlNode rootMaster = masterdoc.DocumentElement;

            XmlDocument updatedoc = new XmlDocument();
            updatedoc.Load(updatefilePath);
            XmlNode rootUpdate = updatedoc.DocumentElement;

            // Add the namespace.  
            //XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            //nsmgr.AddNamespace("bk", "urn:newbooks-schema");

            // Select all nodes data in Master
            bool updateFileChanged = false;
            bool masterFileChanged = false;
            XmlNodeList nodeListMaster = rootMaster.SelectNodes("//data");
            foreach (XmlNode dataMaster in nodeListMaster)
            {
                //Add comment node to master if that is set in settings
                if (addMasterCommentNode)
                {
                    var commentNode = dataMaster.SelectSingleNode("comment");
                    if (commentNode == null)
                    {
                        XmlElement elem = masterdoc.CreateElement("comment"); //item1 ,item2..
                        elem.InnerText = Globals.STATUS_COMMENT_NEED_REVIEW;
                        dataMaster.AppendChild(elem);
                        masterFileChanged = true;
                    }
                    else
                    {
                        if (commentNode.InnerText != Globals.STATUS_COMMENT_NEED_REVIEW)
                            masterFileChanged = true;

                        commentNode.InnerText = Globals.STATUS_COMMENT_NEED_REVIEW;
                    }

                }

                XmlNode exist = rootUpdate.SelectSingleNode("//data[@name='" + dataMaster.Attributes.GetNamedItem("name").Value + "']");
                if (exist == null)
                {
                    //Add to file
                    updateFileChanged = true;
                    XmlNode newEntry = updatedoc.ImportNode(dataMaster, true);
                    updatedoc.DocumentElement.AppendChild(newEntry);
                    //check if comment exist from master
                    var commentNode = newEntry.SelectSingleNode("comment");
                    if (commentNode == null)
                    {
                        XmlElement elem = updatedoc.CreateElement("comment"); //item1 ,item2..
                        elem.InnerText = Globals.STATUS_COMMENT_NEW;
                        newEntry.AppendChild(elem);
                    }
                    else
                    {
                        commentNode.InnerText = Globals.STATUS_COMMENT_NEW;
                    }
                }
                else
                {
                    //Check if comment exist or not
                    var commentNode = exist.SelectSingleNode("comment");
                    if (commentNode == null)
                    {   //If comment not exists then we think this is old row that are allredy translated and final
                        XmlElement elem = updatedoc.CreateElement("comment"); //item1 ,item2..
                        elem.InnerText = Globals.STATUS_COMMENT_FINAL;
                        exist.AppendChild(elem);
                        updateFileChanged = true;
                    }
                    else
                    {  //Comment not ok set to need review
                        if (commentNode.InnerText != Globals.STATUS_COMMENT_NEW && commentNode.InnerText != Globals.STATUS_COMMENT_NEED_REVIEW && commentNode.InnerText != Globals.STATUS_COMMENT_FINAL)
                        {
                            updateFileChanged = true;
                            commentNode.InnerText = Globals.STATUS_COMMENT_NEED_REVIEW;
                        }
                    }
                }


                progress.Pulse();


            }
            //Select all rows data in Updatefile to remove row that not exist in master 
            XmlNodeList nodeListUpdate = rootUpdate.SelectNodes("//data");
            foreach (XmlNode dataUpdate in nodeListUpdate)
            {
                // <data name="Select_All" xml:space="preserve">
                //< value > Select All </ value >
                //<comment>New,Translated,Finish</comment>
                //  </ data >

                XmlNode exist = rootMaster.SelectSingleNode("//data[@name='" + dataUpdate.Attributes.GetNamedItem("name").Value + "']");
                if (exist == null)
                {
                    //remove from file
                    updateFileChanged = true;
                    rootUpdate.RemoveChild(dataUpdate);

                }

                progress.Pulse();



            }
            if (updateFileChanged)
                updatedoc.Save(updatefilePath);

            if (masterFileChanged)
                masterdoc.Save(masterfilePath);

            return new Result<bool>(true);


        }

    }
}
