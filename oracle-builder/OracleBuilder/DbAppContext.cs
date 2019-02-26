using Gov.Jag.Embc.Interfaces.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace OracleBuilder
{
    /// <summary>
    /// Database Context Factory Interface
    /// </summary>
    public interface IDbAppContextFactory
    {
        /// <summary>
        /// Create new database context
        /// </summary>
        /// <returns></returns>
        IDbAppContext Create();
    }

    /// <summary>
    /// Database Context Factory
    /// </summary>
    public class DbAppContextFactory : IDbAppContextFactory
    {
        private readonly DbContextOptions<DbAppContext> _options;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Database Context Factory Constructor
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="options"></param>
        public DbAppContextFactory(IHttpContextAccessor httpContextAccessor, DbContextOptions<DbAppContext> options)
        {
            _options = options;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Create new database context
        /// </summary>
        /// <returns></returns>
        public IDbAppContext Create()
        {
            return new DbAppContext( _options);
        }
    }

    /// <summary>
    /// Database Context Interface
    /// </summary>
    public interface IDbAppContext
    {
        
        DbSet<Contact> Contacts { get; set; }
        DbSet<Account> Accounts { get; set; }
        DbSet<Incident> Incidents { get; set; }
        DbSet<Region> Regions { get; set; }
        DbSet<Registration> Registrations { get; set; }
        DbSet<IncidentTask> IncidentTasks { get; set; }
        DbSet<Community> Communities { get; set; }
        DbSet<Systemuser> Systemusers { get; set; }

    }

    /// <summary>
    /// Database Context Interface
    /// </summary>
    public class DbAppContext : DbContext, IDbAppContext
    {        

        /// <summary>
        /// Constructor for Class used for Entity Framework access.
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="options"></param>
        public DbAppContext(DbContextOptions<DbAppContext> options) : base(options)
        {            
            // override the default timeout as some operations are time intensive
            Database?.SetCommandTimeout(180);
        }

        /// <summary>
        /// Override for OnModelCreating - used to change the database naming convention.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Community>()
                .HasOne(c => c.PrimaryContact)
                .WithMany();

            modelBuilder.Entity<Community>()
                .HasMany(c => c.CommunityEvacueeContacts);

            modelBuilder.Entity<Contact>()
                .HasOne(c => c.Facility)
                .WithMany();

            modelBuilder.Entity<Contact>()
                .HasOne(c => c.Region)
                .WithMany();

            modelBuilder.Entity<Contact>()
                .HasOne(c => c.Community)
                .WithMany();

            modelBuilder.Entity<Contact>()
                .HasMany(c => c.EvacueeContactRegistrations);

            modelBuilder.Entity<Contact>()
                .HasMany(c => c.VolunteerContactRegistrations);

            modelBuilder.Entity<Contact>()
                .HasMany(c => c.PrimaryContactFacilities);

            modelBuilder.Entity<Contact>()
                .HasMany(c => c.PrimaryContactIncidentTasks);

            modelBuilder.Entity<Contact>()
               .HasMany(c => c.PrimaryContactCommunities);

            modelBuilder.Entity<Contact>()
               .HasMany(c => c.PrimaryContactRegions);

            modelBuilder.Entity<Equipment>()
                .HasOne(c => c.EMBCRepresentative)
                .WithMany();

            modelBuilder.Entity<Region>()
                .HasOne(c => c.EMBCRepresentative)
                .WithMany();

            modelBuilder.Entity<Registration>()
                .HasOne(c => c.Evacuee)
                .WithMany();

            modelBuilder.Entity<Registration>()
                .HasOne(c => c.Volunteer)
                .WithMany();

            



            /*
            modelBuilder.Entity<MicrosoftDynamicsCRMcontact>()
                .HasKey(c => c.Contactid);
            modelBuilder.Entity<MicrosoftDynamicsCRMaccount>()
                .HasKey(c => c.Accountid);
            modelBuilder.Entity<MicrosoftDynamicsCRMincident>()
                .HasKey(c => c.Incidentid);
            modelBuilder.Entity<MicrosoftDynamicsCRMinvoice>()
                .HasKey(c => c.Invoiceid);
            modelBuilder.Entity<MicrosoftDynamicsCRMgovRegion>()
                .HasKey(c => c.GovRegionid);
            modelBuilder.Entity<MicrosoftDynamicsCRMsystemuser>()
                .HasKey(c => c.Systemuserid);
            modelBuilder.Entity<MicrosoftDynamicsCRMBooleanManagedProperty>()
                .HasKey(c => c.ManagedPropertyLogicalName);
            modelBuilder.Entity<MicrosoftDynamicsCRMaccountleads>()
                .HasKey(c => c.Accountleadid);

            modelBuilder.Entity<MicrosoftDynamicsCRMactivitymimeattachment>()
                .HasKey(c => c.Activitymimeattachmentid);

            modelBuilder.Entity<MicrosoftDynamicsCRMadminsettingsentity>()
                .HasKey(c => c.Adminsettingsentityid);

            modelBuilder.Entity<MicrosoftDynamicsCRMannotation>()
                .HasKey(c => c.Annotationid);

            modelBuilder.Entity<MicrosoftDynamicsCRMannualfiscalcalendar>()
                .HasKey(c => c.);

            


            modelBuilder.Entity<MicrosoftDynamicsCRMactivitypointer>()
                .HasKey(c => c.Activityid);



            modelBuilder.Entity<MicrosoftDynamicsCRMgovRegistration>()
                            .HasKey(c => c.GovRegistrationid);
            modelBuilder.Entity<MicrosoftDynamicsCRMgovCommunity>()
                            .HasKey(c => c.GovCommunityid);
            modelBuilder.Entity<MicrosoftDynamicsCRMsystemuser>()
                            .HasKey(c => c.Systemuserid);
            modelBuilder.Entity<MicrosoftDynamicsCRMbusinessunit>()
                            .HasKey(c => c.Businessunitid);
            modelBuilder.Entity<MicrosoftDynamicsCRMbusinessunit>()
                .HasOne(c => c.Modifiedby);
            modelBuilder.Entity<MicrosoftDynamicsCRMbusinessunit>()
                .HasOne(c => c.Createdby);
            modelBuilder.Entity<MicrosoftDynamicsCRMbusinessunit>()
                .HasOne(c => c.Createdonbehalfby);
            modelBuilder.Entity<MicrosoftDynamicsCRMbusinessunit>()
                .HasOne(c => c.Modifiedonbehalfby);


            modelBuilder.Entity<MicrosoftDynamicsCRMgovCommunity>()
                            .HasKey(c => c.GovCommunityid);

           
            
            modelBuilder.Entity<MicrosoftDynamicsCRMcalendar>()
                            .HasKey(c => c.Calendarid);
            modelBuilder.Entity<MicrosoftDynamicsCRMcalendar>()
                .HasOne(c => c.Modifiedby);
            modelBuilder.Entity<MicrosoftDynamicsCRMcalendar>()
                .HasOne(c => c.Createdby);
            modelBuilder.Entity<MicrosoftDynamicsCRMcalendar>()
                .HasOne(c => c.Createdonbehalfby);
            modelBuilder.Entity<MicrosoftDynamicsCRMcalendar>()
                .HasOne(c => c.Modifiedonbehalfby);

            modelBuilder.Entity<MicrosoftDynamicsCRMconnection>()
                .HasKey(c => c.Connectionid);
            modelBuilder.Entity<MicrosoftDynamicsCRMconnection>()
                .HasOne(c => c.Modifiedby);
            modelBuilder.Entity<MicrosoftDynamicsCRMconnection>()
                .HasOne(c => c.Createdby);
            modelBuilder.Entity<MicrosoftDynamicsCRMconnection>()
                .HasOne(c => c.Createdonbehalfby);
            modelBuilder.Entity<MicrosoftDynamicsCRMconnection>()
                .HasOne(c => c.Modifiedonbehalfby);
            modelBuilder.Entity<MicrosoftDynamicsCRMconnection>()
                .HasOne(c => c.Record1idAccount);
            modelBuilder.Entity<MicrosoftDynamicsCRMconnection>()
                .HasOne(c => c.Record1idActivitypointer);
            modelBuilder.Entity<MicrosoftDynamicsCRMconnection>()
                .HasOne(c => c.Record1idAppointment);
            modelBuilder.Entity<MicrosoftDynamicsCRMconnection>()
                .HasOne(c => c.Record1idCampaign);
            modelBuilder.Entity<MicrosoftDynamicsCRMconnection>()
                .HasOne(c => c.Record1idCampaignactivity);
            modelBuilder.Entity<MicrosoftDynamicsCRMconnection>()
                .HasOne(c => c.Record1idCompetitor);
            modelBuilder.Entity<MicrosoftDynamicsCRMconnection>()
                .HasOne(c => c.Record1idConstraintbasedgroup);
            modelBuilder.Entity<MicrosoftDynamicsCRMconnection>()
                .HasOne(c => c.Record1idContact);
            modelBuilder.Entity<MicrosoftDynamicsCRMconnection>()
                .HasOne(c => c.Record1idContract);
            modelBuilder.Entity<MicrosoftDynamicsCRMconnection>()
                .HasOne(c => c.Record1idEmail);
            modelBuilder.Entity<MicrosoftDynamicsCRMconnection>()
                .HasOne(c => c.Record1idEntitlement);
            modelBuilder.Entity<MicrosoftDynamicsCRMconnection>()
                .HasOne(c => c.Record1idEntitlementchannel);
            modelBuilder.Entity<MicrosoftDynamicsCRMconnection>()
                .HasOne(c => c.Record1idEntitlementtemplatechannel);
            modelBuilder.Entity<MicrosoftDynamicsCRMconnection>()
                .HasOne(c => c.Record1idEquipment);
            modelBuilder.Entity<MicrosoftDynamicsCRMconnection>()
                .HasOne(c => c.Record1idFax);
            modelBuilder.Entity<MicrosoftDynamicsCRMconnection>()
                .HasOne(c => c.Record1idGoal);
            modelBuilder.Entity<MicrosoftDynamicsCRMconnection>()
                .HasOne(c => c.Record1idGovIncidenttask);
            modelBuilder.Entity<MicrosoftDynamicsCRMconnection>()
                .HasOne(c => c.Record1idGovRegistration);
            modelBuilder.Entity<MicrosoftDynamicsCRMconnection>()
                .HasOne(c => c.Record1idIncident);
            modelBuilder.Entity<MicrosoftDynamicsCRMconnection>()
                .HasOne(c => c.Record1idProcesssession);
            modelBuilder.Entity<MicrosoftDynamicsCRMconnection>()
                .HasOne(c => c.Record2idProcesssession);

            /***************
             * 
             * CONTACT
             * 
             ***************/
            /*
           //'MicrosoftDynamicsCRMcontact.GovFacilityId' of type 'MicrosoftDynamicsCRMequipment
           modelBuilder.Entity<MicrosoftDynamicsCRMcontact>()
               .HasOne(c => c.GovFacilityId);

           //'MicrosoftDynamicsCRMcontact.Originatingleadid' of type 'MicrosoftDynamicsCRMlead
           modelBuilder.Entity<MicrosoftDynamicsCRMcontact>()
               .HasOne(c => c.Originatingleadid);

           //'MicrosoftDynamicsCRMcontact.Preferredequipmentid' of type 'MicrosoftDynamicsCRMequipment'.Either manually configure the
           modelBuilder.Entity<MicrosoftDynamicsCRMcontact>()
               .HasOne(c => c.Preferredequipmentid);

           modelBuilder.Entity<MicrosoftDynamicsCRMcontact>()
               .HasOne(c => c.GovVolunteerId);

           modelBuilder.Entity<MicrosoftDynamicsCRMcontact>()
               .Ignore(c => c.Masterid);

           //'MicrosoftDynamicsCRMequipment.GovPrimaryContactId' of type 'MicrosoftDynamicsCRMcontact'
           modelBuilder.Entity<MicrosoftDynamicsCRMequipment>()
               .HasOne(c => c.GovPrimaryContactId);

           /***************
            * 
            * INCIDENT
            * 
            ***************/
            /*
           modelBuilder.Entity<MicrosoftDynamicsCRMincident>()
               .HasOne(c => c.Firstresponsebykpiid);

           modelBuilder.Entity<MicrosoftDynamicsCRMincident>()
               .HasOne(c => c.Resolvebykpiid);

           modelBuilder.Entity<MicrosoftDynamicsCRMincident>()
               .Ignore(c => c.Existingcase);



           modelBuilder.Entity < MicrosoftDynamicsCRMlead>()
               .HasOne(c => c.CustomeridContact);

           modelBuilder.Entity<MicrosoftDynamicsCRMlead>()
               .HasOne(c => c.Parentcontactid);

           // 'MicrosoftDynamicsCRMmailbox.Createdby' of type 'MicrosoftDynamicsCRMsystemuser

           modelBuilder.Entity<MicrosoftDynamicsCRMmailbox>()
               .HasOne(c => c.Modifiedby);
           modelBuilder.Entity<MicrosoftDynamicsCRMmailbox>()
               .HasOne(c => c.Createdby);
           modelBuilder.Entity<MicrosoftDynamicsCRMmailbox>()
               .HasOne(c => c.Createdonbehalfby);
           modelBuilder.Entity<MicrosoftDynamicsCRMmailbox>()
               .HasOne(c => c.Modifiedonbehalfby);

           modelBuilder.Entity<MicrosoftDynamicsCRMmailbox>()
               .HasOne(c => c.Owninguser);

           modelBuilder.Entity<MicrosoftDynamicsCRMmailbox>()
               .Ignore(c => c.Regardingobjectid);

           modelBuilder.Entity<MicrosoftDynamicsCRMmobileofflineprofile>()
               .HasOne(c => c.Modifiedby);
           modelBuilder.Entity<MicrosoftDynamicsCRMmobileofflineprofile>()
               .HasOne(c => c.Createdby);
           modelBuilder.Entity<MicrosoftDynamicsCRMmobileofflineprofile>()
               .HasOne(c => c.Createdonbehalfby);
           modelBuilder.Entity<MicrosoftDynamicsCRMmobileofflineprofile>()
               .HasOne(c => c.Modifiedonbehalfby);

           //'MicrosoftDynamicsCRMorganization.Modifiedonbehalfby' of type 'MicrosoftDynamicsCRMsystemuser
           modelBuilder.Entity<MicrosoftDynamicsCRMorganization>()
               .HasOne(c => c.Modifiedby)
               .WithMany()
               .HasForeignKey(x => x.Systemuserid);
           modelBuilder.Entity<MicrosoftDynamicsCRMorganization>()
               .HasOne(c => c.Createdby)
               .WithMany()
               .HasForeignKey(x => x.Systemuserid);
           modelBuilder.Entity<MicrosoftDynamicsCRMorganization>()
               .HasOne(c => c.Createdonbehalfby);
           modelBuilder.Entity<MicrosoftDynamicsCRMorganization>()
               .HasOne(c => c.Modifiedonbehalfby);

           modelBuilder.Entity<MicrosoftDynamicsCRMposition>()
               .HasOne(c => c.Modifiedby);
           modelBuilder.Entity<MicrosoftDynamicsCRMposition>()
               .HasOne(c => c.Createdby);
           modelBuilder.Entity<MicrosoftDynamicsCRMposition>()
               .HasOne(c => c.Createdonbehalfby);
           modelBuilder.Entity<MicrosoftDynamicsCRMposition>()
               .HasOne(c => c.Modifiedonbehalfby);

           modelBuilder.Entity<MicrosoftDynamicsCRMprocesssession>()
               .HasOne(c => c.RegardingobjectidConnection);

           modelBuilder.Entity<MicrosoftDynamicsCRMqueue>()
               .HasOne(c => c.Primaryuserid);
           modelBuilder.Entity<MicrosoftDynamicsCRMqueue>()
               .HasOne(c => c.Modifiedby);
           modelBuilder.Entity<MicrosoftDynamicsCRMqueue>()
               .HasOne(c => c.Createdby);
           modelBuilder.Entity<MicrosoftDynamicsCRMqueue>()
               .HasOne(c => c.Createdonbehalfby);
           modelBuilder.Entity<MicrosoftDynamicsCRMqueue>()
               .HasOne(c => c.Modifiedonbehalfby);
           // MicrosoftDynamicsCRMqueue.Createdby' 

           modelBuilder.Entity<MicrosoftDynamicsCRMsite>()
               .HasOne(c => c.Modifiedby);
           modelBuilder.Entity<MicrosoftDynamicsCRMsite>()
               .HasOne(c => c.Createdby);
           modelBuilder.Entity<MicrosoftDynamicsCRMsite>()
               .HasOne(c => c.Createdonbehalfby);
           modelBuilder.Entity<MicrosoftDynamicsCRMsite>()
               .HasOne(c => c.Modifiedonbehalfby);

           // MicrosoftDynamicsCRMslakpiinstance.Regarding' of type 'MicrosoftDynamicsCRMincident'. Either manually configure the relationship
           modelBuilder.Entity<MicrosoftDynamicsCRMslakpiinstance>()
               .Ignore(c => c.Regarding);

           modelBuilder.Entity<MicrosoftDynamicsCRMsystemuser>()
               .HasOne(c => c.Modifiedby);
           modelBuilder.Entity<MicrosoftDynamicsCRMsystemuser>()
               .HasOne(c => c.Createdby);
           modelBuilder.Entity<MicrosoftDynamicsCRMsystemuser>()
               .HasOne(c => c.Createdonbehalfby);
           modelBuilder.Entity<MicrosoftDynamicsCRMsystemuser>()
               .HasOne(c => c.Modifiedonbehalfby);

           modelBuilder.Entity<MicrosoftDynamicsCRMsystemuser>()
               .HasOne(c => c.Territoryid);

           modelBuilder.Entity<MicrosoftDynamicsCRMsystemuser>()
               .HasOne(c => c.Siteid);

           modelBuilder.Entity<MicrosoftDynamicsCRMsystemuser>()
               .HasOne(c => c.Defaultmailbox);

           modelBuilder.Entity<MicrosoftDynamicsCRMsystemuser>()
               .HasOne(c => c.Positionid);

           modelBuilder.Entity<MicrosoftDynamicsCRMsystemuser>()
               .HasOne(c => c.Transactioncurrencyid);

           modelBuilder.Entity<MicrosoftDynamicsCRMsystemuser>()
               .Ignore(c => c.OrganizationidOrganization);

           modelBuilder.Entity<MicrosoftDynamicsCRMsystemuser>()
               .HasOne(c => c.Calendarid);

           modelBuilder.Entity<MicrosoftDynamicsCRMsystemuser>()
               .HasOne(c => c.Mobileofflineprofileid);

           modelBuilder.Entity<MicrosoftDynamicsCRMsystemuser>()
               .HasOne(c => c.Queueid);

           modelBuilder.Entity<MicrosoftDynamicsCRMsystemuser>()
               .HasOne(c => c.Businessunitid);

           modelBuilder.Entity<MicrosoftDynamicsCRMsystemuser>()
               .Ignore(c => c.Parentsystemuserid);


           modelBuilder.Entity<MicrosoftDynamicsCRMterritory>()
               .HasOne(c => c.Modifiedby);
           modelBuilder.Entity<MicrosoftDynamicsCRMterritory>()
               .HasOne(c => c.Createdby);
           modelBuilder.Entity<MicrosoftDynamicsCRMterritory>()
               .HasOne(c => c.Createdonbehalfby);
           modelBuilder.Entity<MicrosoftDynamicsCRMterritory>()
               .HasOne(c => c.Modifiedonbehalfby);           

           modelBuilder.Entity<MicrosoftDynamicsCRMterritory>()
               .HasOne(c => c.Managerid);

           modelBuilder.Entity<MicrosoftDynamicsCRMtransactioncurrency>()
               .HasOne(c => c.Modifiedby);
           modelBuilder.Entity<MicrosoftDynamicsCRMtransactioncurrency>()
               .HasOne(c => c.Createdby);
           modelBuilder.Entity<MicrosoftDynamicsCRMtransactioncurrency>()
               .HasOne(c => c.Createdonbehalfby);
           modelBuilder.Entity<MicrosoftDynamicsCRMtransactioncurrency>()
               .HasOne(c => c.Modifiedonbehalfby);

           modelBuilder.Entity<MicrosoftDynamicsCRMasyncoperation>()
               .Ignore(c => c.RegardingobjectidEmail);

           //MicrosoftDynamicsCRMemail.RegardingobjectidAsyncoperation
           modelBuilder.Entity<MicrosoftDynamicsCRMemail>()
               .Ignore("RegardingobjectidAsyncoperation");

           modelBuilder.Entity<MicrosoftDynamicsCRMsocialactivity>()
               .Ignore("RegardingobjectidAsyncoperation");

           modelBuilder.Entity<MicrosoftDynamicsCRMactivitypointer>()
               .Ignore("RegardingobjectidBulkoperation");

           modelBuilder.Entity<MicrosoftDynamicsCRMbusinessunit>()
               .Ignore("Calendarid");

           modelBuilder.Entity<MicrosoftDynamicsCRMactivitypointer>()
               .Ignore("RegardingobjectidCampaignactivity");

           modelBuilder.Entity<MicrosoftDynamicsCRMlead>()
               .Ignore("Relatedobjectid");

           modelBuilder.Entity<MicrosoftDynamicsCRMcontact>()
               .Ignore("ParentcustomeridAccount");

           modelBuilder.Entity<MicrosoftDynamicsCRMdynamicproperty>()
               .Ignore("Dynamicpropertyoptionsetvalueid");

           modelBuilder.Entity<MicrosoftDynamicsCRMemailserverprofile>()
               .Ignore("Organizationid");

           modelBuilder.Entity<MicrosoftDynamicsCRMcontact>()
               .Ignore("GovCommunityId");

           modelBuilder.Entity<MicrosoftDynamicsCRMcontact>()
               .Ignore("GovRegionId");

           modelBuilder.Entity<MicrosoftDynamicsCRMteam>()
               .Ignore("RegardingobjectidKnowledgearticle");

           modelBuilder.Entity<MicrosoftDynamicsCRMopportunity>()
               .Ignore("Originatingleadid");

           modelBuilder.Entity<MicrosoftDynamicsCRMteam>()
               .Ignore("RegardingobjectidOpportunity");

           modelBuilder.Entity<MicrosoftDynamicsCRMtransactioncurrency>()
               .Ignore("Organizationid");

           modelBuilder.Entity<MicrosoftDynamicsCRMcalendar>()
               .Ignore("Organizationid");


           modelBuilder.Entity<MicrosoftDynamicsCRMmobileofflineprofile>()
               .Ignore("Organizationid");

           modelBuilder.Entity<MicrosoftDynamicsCRMmailbox>()
               .Ignore("RegardingobjectidQueue");

           modelBuilder.Entity<MicrosoftDynamicsCRMknowledgearticle>()
               .Ignore("PreviousArticleContentId");

           modelBuilder.Entity<MicrosoftDynamicsCRMprocesssession>()
               .Ignore("Previouslinkedsessionid");

           */


            //            System.InvalidOperationException: 'Both relationships between 'MicrosoftDynamicsCRMorganization.Modifiedby' and 'MicrosoftDynamicsCRMsystemuser' and between 'MicrosoftDynamicsCRMorganization.Createdby' and 'MicrosoftDynamicsCRMsystemuser' could use {'Systemuserid'} as the foreign key. To resolve this configure the foreign key properties explicitly on at least one of the relationships.'


            //           'MicrosoftDynamicsCRMsystemuser.OrganizationidOrganization' of
            // add our naming convention extension
            modelBuilder.UpperCaseUnderscoreSingularConvention();
        }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Incident> Incidents { get; set; }
        public DbSet<IncidentTask> IncidentTasks { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<Community> Communities { get; set; }
        public DbSet<Systemuser> Systemusers { get; set; }
    }
}
