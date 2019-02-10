using DataEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Website.Models
{
    public class RequestSubmitModel
    {
        public string Description { get; set; }
        public DateTime DateSubmitted { get; set; }
        public string ErrorMessage { get; set; }
        public bool Submitted { get; set; }
        public IEnumerable<SelectListItem> RequestTypeOptions { get; set; }
        public string SelectedRequestId { get; set; }
        public IEnumerable<RequestSubmission> Submissions { get; set; }

        public RequestSubmitModel()
        {
            Submitted = false;
            Description = "";
            DateSubmitted = DateTime.Now;
            ErrorMessage = null;
            SelectedRequestId = null;

            GetTypeOptions();
            GetSubmissions();
        }

        public void AddToDB()
        {
            bool safeToSave = true;
            if (Description == null && Description.Length < 10)
            {
                ErrorMessage = "Description must be at least 20 characters to help me fully understand what you need.";
                safeToSave = false;
            }

            using (BeaujeauxEntities database = new BeaujeauxEntities())
            {
                var id = int.Parse(SelectedRequestId);
                RequestType requestType = database.RequestTypes.Where(s => s.Id == id).FirstOrDefault();
                if (requestType == null)
                {
                    safeToSave = false;
                    ErrorMessage = "Please select a request type.";
                }

                if (safeToSave)
                {
                    database.RequestSubmissions.Add(new RequestSubmission()
                    {
                        RequestTypeId = requestType.Id,
                        Description = Description,
                        DateSubmitted = DateTime.Now,
                        CompleteComment = null,
                        DateComplete = null,
                    });
                    database.SaveChanges();
                }
            }

            Submitted = safeToSave;
        }

        public void GetSubmissions()
        {
            using (BeaujeauxEntities database = new BeaujeauxEntities())
            {
                Submissions = database.RequestSubmissions
                    .Where(a => a.DateComplete == null)
                    .OrderByDescending(t => t.DateSubmitted)
                    .Include(t => t.RequestType)
                    .ToList();
            }
        }

        private void GetTypeOptions()
        {
            using (BeaujeauxEntities database = new BeaujeauxEntities())
            {
                var requestTypes = database.RequestTypes.OrderBy(a => a.Rank);
                List<SelectListItem> options = new List<SelectListItem>();

                int id = SelectedRequestId == null ? 0 : int.Parse(SelectedRequestId);

                options.Add(new SelectListItem()
                {
                    Text = "Select an Option",
                    Value = "0",
                    Selected = id == 0,
                });

                foreach (var type in requestTypes)
                {
                    options.Add(new SelectListItem()
                    {
                        Text = type.Display,
                        Value = type.Id.ToString(),
                        Selected = id == type.Id,
                    });
                }
                RequestTypeOptions = options;
            }
        }


    }
}