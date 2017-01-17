using MachineInfo;
using ProductionScheduleSheet.Models;
using ProductionScheduleSheet.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Caching;
using System.Web.Http;

namespace ProductionScheduleSheet.Controllers
{
    public class ValueController : ApiController
    {
        private ProductEntities db = new ProductEntities();

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpGet]
        public HttpResponseMessage GetDB()
        {
            try
            {
                var infos = db.MachineInfoes.Where(x => x.MachineStatus == "WIP" || x.MachineStatus == "OPEN");
                List<MachineInfo.MachineInfo> MachineInfos = new List<MachineInfo.MachineInfo>();
                foreach(var i in infos)
                {
                    MachineInfos.Add(i);
                }
                FileSerialize.SafeXmlSerializeToFile<List<MachineInfo.MachineInfo>>(HttpContext.Current.Server.MapPath("~/App_Data/DbData.xml"), MachineInfos);
                return Request.CreateResponse(HttpStatusCode.OK, db.MachineInfoes.Where(x => x.MachineStatus == "WIP" || x.MachineStatus == "OPEN"));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, ex.ToString());
            }
            //return new string[] { "value1", "value2" };
        }
        [HttpGet]
        public HttpResponseMessage GetXml()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, FileSerialize.SafeXmlDeserializeFromFile<ScheduleEdit>(HttpContext.Current.Server.MapPath("~/App_Data/ScheduleEdit.xml")));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, ex.ToString());
            }
            //return new string[] { "value1", "value2" };
        }
        [HttpGet]
        public HttpResponseMessage GetDBXml()
        {
            try
            {
                var MachineInfos = FileSerialize.SafeXmlDeserializeFromFile<List<MachineInfo.MachineInfo>>(HttpContext.Current.Server.MapPath("~/App_Data/DbData.xml"));
                return Request.CreateResponse(HttpStatusCode.OK, MachineInfos);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, ex.ToString());
            }
            //return new string[] { "value1", "value2" };
        }
        [HttpGet]
        public HttpResponseMessage MachineInfos()
        {
            try
            {
                ScheduleEdit model = FileSerialize.SafeXmlDeserializeFromFile<ScheduleEdit>(HttpContext.Current.Server.MapPath("~/App_Data/ScheduleEdit.xml"));
                var list = db.MachineInfoes.Where(x => x.MachineStatus == "WIP" || x.MachineStatus == "OPEN");
                return SyncList(model, list);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.ToString());
            }
        }
        private HttpResponseMessage SyncList(ScheduleEdit model, IQueryable<MachineInfo.MachineInfo> list)
        {
            var listOutput = new List<OutputRecord>();

            if (model.Items.Count == 0)
            {
                foreach (var item in list)
                {
                    listOutput.Add(new OutputRecord(item));
                }
                HttpRuntime.Cache.Insert("listOutput", listOutput, null, Cache.NoAbsoluteExpiration, TimeSpan.FromHours(1));
                return Request.CreateResponse(HttpStatusCode.OK, listOutput);
            }
            foreach (var item in list)
            {
                var it = model.GetItem(item.JobNum);
                if (it != null)
                {
                    listOutput.Add(new OutputRecord(item, it, BOMSequence(), DocSequence(), ReviewSequence(), ProgramSequence(), EPrintSequence()));
                }
                else
                {
                    listOutput.Add(new OutputRecord(item));
                }

            }
            HttpRuntime.Cache.Insert("listOutput", listOutput, null, Cache.NoAbsoluteExpiration, TimeSpan.FromHours(1));
                return Request.CreateResponse(HttpStatusCode.OK, listOutput);
        }

        [HttpPut]
        //[ResponseType(typeof(void))]
        public HttpResponseMessage Update(OutputRecord rec)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
            }
            var list = (List<OutputRecord>)HttpRuntime.Cache["listOutput"];
            if (list.Count(r => r.JobNum == rec.JobNum) == 0)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            try
            {
                var theRec = list.Where(r => r.JobNum == rec.JobNum).FirstOrDefault();
                theRec.Update(rec);
                UpdateChanges(list);
                HttpRuntime.Cache.Insert("listOutput", list, null, Cache.NoAbsoluteExpiration, TimeSpan.FromHours(1));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        private void UpdateChanges(List<OutputRecord> list)
        {
            var xmlModel = new ScheduleEdit();
            foreach (var item in list)
            {
                xmlModel.Items.Add(new ScheduleItem()
                {
                    JobNum = item.JobNum,
                    WarningDate = item.WarningDate,
                    BOMState = item.BOMState,
                    Documentation = item.Documentation,
                    EprintOwner = item.EprintOwner,
                    EprintState = item.EprintState,
                    ProgramOwner = item.ProgramOwner,
                    ProgramState = item.ProgramState,
                    ReviewState = item.ReviewState
                });
            }
            FileSerialize.SafeXmlSerializeToFile<ScheduleEdit>(HttpContext.Current.Server.MapPath("~/App_Data/ScheduleEdit.xml"), xmlModel);
        }

        [HttpGet]
        public IQueryable<SelectItem> DocSequence()
        {
            var model = (BaseSelection)HttpRuntime.Cache["DocSequence"];
            if (model == null)
            {
                model = FileSerialize.SafeXmlDeserializeFromFile<BaseSelection>(HttpContext.Current.Server.MapPath("~/App_Data/DocSequence.xml"));
                HttpRuntime.Cache.Insert("DocSequence", model, null, DateTime.Now.AddDays(1), Cache.NoSlidingExpiration);

            }
            return model.SelectItems.AsQueryable();
        }

        [HttpGet]
        public IQueryable<SelectItem> BOMSequence()
        {
            var model = (BaseSelection)HttpRuntime.Cache["BOMSequence"];
            if (model == null)
            {
                model = FileSerialize.SafeXmlDeserializeFromFile<BaseSelection>(HttpContext.Current.Server.MapPath("~/App_Data/BOMSequence.xml"));
                HttpRuntime.Cache.Insert("BOMSequence", model, null, DateTime.Now.AddDays(1), Cache.NoSlidingExpiration);

            }
            return model.SelectItems.AsQueryable();
        }

        [HttpGet]
        public IQueryable<SelectItem> EPrintSequence()
        {
            var model = (BaseSelection)HttpRuntime.Cache["EPrintSequence"];
            if (model == null)
            {
                model = FileSerialize.SafeXmlDeserializeFromFile<BaseSelection>(HttpContext.Current.Server.MapPath("~/App_Data/EPrintSequence.xml"));
                HttpRuntime.Cache.Insert("EPrintSequence", model, null, DateTime.Now.AddDays(1), Cache.NoSlidingExpiration);

            }
            return model.SelectItems.AsQueryable();
        }

        [HttpGet]
        public IQueryable<SelectItem> ProgramSequence()
        {
            var model = (BaseSelection)HttpRuntime.Cache["ProgramSequence"];
            if (model == null)
            {
                model = FileSerialize.SafeXmlDeserializeFromFile<BaseSelection>(HttpContext.Current.Server.MapPath("~/App_Data/ProgramSequence.xml"));
                HttpRuntime.Cache.Insert("ProgramSequence", model, null, DateTime.Now.AddDays(1), Cache.NoSlidingExpiration);

            }
            return model.SelectItems.AsQueryable();
        }

        [HttpGet]
        public IQueryable<SelectItem> ReviewSequence()
        {
            var model = (BaseSelection)HttpRuntime.Cache["ReviewSequence"];
            if (model == null)
            {
                model = FileSerialize.SafeXmlDeserializeFromFile<BaseSelection>(HttpContext.Current.Server.MapPath("~/App_Data/ReviewSequence.xml"));
                HttpRuntime.Cache.Insert("ReviewSequence", model, null, DateTime.Now.AddDays(1), Cache.NoSlidingExpiration);

            }
            return model.SelectItems.AsQueryable();
        }

    }
}