using EasySave_G8_UI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;

namespace EasySave_G8_UI.View_Models
{
    public class View_Model
    {
        //VM Startup Initialisation
        public void VM_Init() 
        {
            Model_COMMON ModelCOMMON = new Model_COMMON("en");
            ModelCOMMON.Init(ModelCOMMON);
        }

        //VM Classic Save
        public void VM_Classic(string Name, string Source, string Destination, bool Type)
        {
            Model_PRE ModelPRE = new Model_PRE(Name, Source, Destination, Type);
            ModelPRE.Exec(null);
        }

        //VM Check if a Work exists
        public bool VM_Work_Exist(string Name)
        {
            string fileName = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\EasySave\works_config.json";
            if (File.Exists(fileName))
            {
                Model_Works ModelWorks = new Model_Works();
                List<Model_PRE>? obj_list = (ModelWorks.Get_Work(Name, false));
                if (!obj_list.Any()) { return false; } //Check if there is content in the list, if not that means not object with the given name was found
                else { return true; }
            }
            return false;
        }

        //VM Returns a list of Work objects
        public List<Model_PRE> VM_Work_Show(string? Name, bool AllWorks)
        {
            Model_Works ModelWorks = new Model_Works();
            List<Model_PRE>? obj_list = (ModelWorks.Get_Work(Name, AllWorks));
            return (obj_list);
        } 

        //VM Create a new work
        public void VM_Work_New(string Name, string Source, string Destination, bool Type, bool ExeNow)
        {
            Model_PRE ModelPRE = new Model_PRE(Name, Source, Destination, Type);
            ModelPRE.Save(ExeNow);
        }

        //VM Run a single or all works
        public void VM_Work_Run(string Name, object sender)
        {
            Model_Works ModelWorks = new Model_Works();
            List<Model_PRE>? obj_list = (ModelWorks.Get_Work(Name, false)); //Use Get_Work to get Work data
            foreach (Model_PRE obj in obj_list) //Loop throught every works in list and execute them
            {
                Model_PRE ModelPRE = new Model_PRE(obj.Name, obj.Source, obj.Destination, obj.Type);
                ModelPRE.Exec(sender);
            }
        }

        //VM Delete a Work
        public void VM_Work_Delete(string Name)
        {
            Model_Works ModelWorks = new Model_Works();
            ModelWorks.Delete_Work(Name);
        }

        //VM Change app_config Language
        public void VM_Change_Language(string lang)
        {
            Model_LANG ModelLANG = new Model_LANG(lang);
            ModelLANG.ChangeLanguage();
        }

        //VM Get app_config Language
        public void VM_Update_Language()
        {
            Model_LANG ModelLANG = new Model_LANG(null);
            ModelLANG.UpdateLanguage();
        }

        //VM Translate string from resx files
        public static string VM_GetString_Language(string trsl_string)
        {
            string rtrn_string = Model_LANG.GetString(trsl_string);
            return rtrn_string;
        }

        //MV Show the daily log files content
        public List<Model_AFT> MV_Look_Logs(string Date)
        {
            Model_Logs ModelLogs = new Model_Logs();
            List<Model_AFT> list = ModelLogs.Get_Logs(Date);
            return list;
        }

        public List<Model_StateLogs> MV_Look_StateLogs()
        {
            Model_Logs ModelLogs = new Model_Logs();
            List<Model_StateLogs> list = ModelLogs.Get_StateLogs();
            return list;
        }

        public bool VM_BlackListTest()
        {
            Model_BLACKLIST ModelBLACKLIST = new Model_BLACKLIST();
            return ModelBLACKLIST.BlacklistTest();
        }

        public void VM_BlackListAdd(string ProcessName)
        {
            Model_BLACKLIST ModelBLACKLIST = new Model_BLACKLIST();
            ModelBLACKLIST.BlacklistAdd(ProcessName);
        }

        public void VM_BlackListRemove(string ProcessNameRm)
        {
            Model_BLACKLIST ModelBLACKLIST = new Model_BLACKLIST();
            ModelBLACKLIST.BlacklistRemove(ProcessNameRm);
        }

        public List<string> MV_Blacklist()
        {
            Model_BLACKLIST ModelBLACKLIST = new Model_BLACKLIST();
            return ModelBLACKLIST.BlacklistReturn();
        }

        public bool VM_StateLogsExists(string Name)
        {
            Model_Logs ModelLOGS = new Model_Logs();
            return ModelLOGS.StatelogExists(Name);
        }

        public void VM_PriorityListAdd(string ProcessName,int Index)
        {
            Model_PRIORITY ModelPRIORITY = new Model_PRIORITY();
            ModelPRIORITY.priorityAdd(ProcessName,Index);
        }

        public void VM_PriorityListRemove(string ProcessNameRm)
        {
            Model_PRIORITY ModelPRIORITY = new Model_PRIORITY();
            ModelPRIORITY.priorityRemove(ProcessNameRm);
        }

        public List<string> MV_PriorityListRe()
        {
            Model_PRIORITY ModelPRIORITY = new Model_PRIORITY();
            return ModelPRIORITY.priorityReturn();
        }
        public void VM_ExtensionListAdd(string ProcessName, int Index)
        {
            Model_EXTENSION ModelEXTENSION = new Model_EXTENSION();
            ModelEXTENSION.ExtensionAdd(ProcessName, Index);
        }

        public void VM_ExtensionListRemove(string ProcessNameRm)
        {
            Model_EXTENSION ModelEXTENSION = new Model_EXTENSION();
            ModelEXTENSION.ExtensionRemove(ProcessNameRm);
        }

        public List<string> MV_ExtensionListRe()
        {
            Model_EXTENSION ModelEXTENSION = new Model_EXTENSION();
            return ModelEXTENSION.priorityReturn();
        }
    }
}