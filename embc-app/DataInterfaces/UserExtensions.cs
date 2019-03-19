using Gov.Jag.Embc.Public.Models;
using Gov.Jag.Embc.Public.Sqlite.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static class UserExtensions
    {
        /// <summary>
        /// Load User from database using their userId and guid
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userId"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static async Task<User> LoadUser(this IDataInterface _dataInterface, string userId, IHeaderDictionary Headers, ILogger _logger, string guid = null)
        {
            User user = null;
            ViewModels.Volunteer contact = null;
            Guid userGuid;

            if (!string.IsNullOrEmpty(guid))
            {
                user = await _dataInterface.GetUserByGuid(guid);
            }

            if (user == null)
            {
                _logger.LogInformation(">>>> LoadUser for BCEID.");
                if (Guid.TryParse(userId, out userGuid))
                {
                    user = _dataInterface.GetUserBySmUserId(userId);
                    if (user != null)
                    {
                        _logger.LogInformation(">>>> LoadUser for BCEID: user != null");
                        
                        // if you wish to update the contact with Siteminder headers, do it here.
                    }
                }
                else
                { //BC service card login

                    _logger.LogInformation(">>>> LoadUser for BC Services Card.");
                    //string externalId = GetServiceCardID(userId);
                    contact = null; //  _dynamicsClient.GetContactByExternalId(externalId);

                    if (contact != null)
                    {
                        _logger.LogInformation(">>>> LoadUser for BC Services Card: contact != null");

                        /*
                        user = new User();
                        user.FromContact(contact);

                        // Update the contact and worker with info from Siteminder
                        var contactVM = new Public.ViewModels.Contact();
                        var workerVm = new Public.ViewModels.Worker();
                        contactVM.CopyHeaderValues(Headers);
                        workerVm.CopyHeaderValues(Headers);
                        MicrosoftDynamicsCRMcontact patchContact = new MicrosoftDynamicsCRMcontact();
                        MicrosoftDynamicsCRMadoxioWorker patchWorker = new MicrosoftDynamicsCRMadoxioWorker();
                        patchContact.CopyValues(contactVM);
                        patchWorker.CopyValues(workerVm);
                        try
                        {
                            string filter = $"_adoxio_contactid_value eq {contact.Contactid}";
                            var workers = _dynamicsClient.Workers.Get(filter: filter).Value;
                            foreach (var item in workers)
                            {
                                _dynamicsClient.Workers.Update(item.AdoxioWorkerid, patchWorker);

                            }
                            _dynamicsClient.Contacts.Update(user.ContactId.ToString(), patchContact);
                        }
                        catch (OdataerrorException odee)
                        {
                            _logger.LogError("Error updating Contact");
                            _logger.LogError("Request:");
                            _logger.LogError(odee.Request.Content);
                            _logger.LogError("Response:");
                            _logger.LogError(odee.Response.Content);
                            // fail if we can't create.
                            throw (odee);
                        }
                        */
                    }
                }
            }

            if (user == null)
                return null;

            if (guid == null)
                return user;


            if (!user.ContactId.ToString().Equals(guid, StringComparison.OrdinalIgnoreCase))
            {
                // invalid account - guid doesn't match user credential
                return null;
            }

            return user;
        }

        /// <summary>
        /// Convert a service card ID string into a format that is useful (and fits into Dynamics)
        /// </summary>
        /// <param name="raw"></param>
        /// <returns></returns>
        public static string GetServiceCardID(string raw)
        {
            string result = "";
            if (!string.IsNullOrEmpty(raw))
            {
                var tokens = raw.Split('|');
                if (tokens.Length > 0)
                {
                    result = tokens[0];
                }

                if (!string.IsNullOrEmpty(result))
                {
                    tokens = result.Split(':');
                    result = tokens[tokens.Length - 1];
                }
            }

            return result;
        }


        /// <summary>
        /// Returns a User based on the guid
        /// </summary>
        /// <param name="context"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static async Task<User> GetUserByGuid(this IDataInterface _dataInterface, string guid)
        {
            Guid id = new Guid(guid);
            User user = null;
            var contact = _dataInterface.GetVolunteerById(guid);
            if (contact != null)
            {
                user = new User();
                user.FromVolunteer(contact);
            }

            return user;
        }



        /// <summary>
        /// Returns a User based on the guid
        /// </summary>
        /// <param name="context"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static User GetUserBySmUserId(this IDataInterface _dataInterface, string guid)
        {
            Guid id = new Guid(guid);
            User user = null;
            var contact = _dataInterface.GetVolunteerByExternalId(id.ToString());
            if (contact != null)
            {
                user = new User();
                user.FromVolunteer(contact);
            }

            return user;
        }

        /// <summary>
        /// Copy values from a Dynamics legal entity to another one
        /// </summary>
        /// <param name="to"></param>
        /// <param name="from"></param>
        public static void FromVolunteer(this User to, ViewModels.Volunteer from)
        {
            if (from.Id != null)
            {
                to.ContactId = Guid.Parse(from.Id);
            }

            if (from.Organization != null && from.Organization.Id != null)
            {
                to.AccountId = Guid.Parse(from.Organization.Id);
            }

            to.GivenName = from.FirstName;
            to.Surname = from.LastName;
            to.SmUserId = from.BceidAccountNumber;
            to.Email = from.Email;
            to.Active = true;
        }

    }

}
