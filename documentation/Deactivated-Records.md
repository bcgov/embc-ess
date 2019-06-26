# Impact of Deactived Records

Below is described the effects to records, which are soft deleted, meaning they are not permanently removed from but are given a value of "Inactive".

### Evacuee

* The evacuee record will not be searchable, nor will it appear in "List of Evacuees" page
* Since they won't appear or be found in a search, the associated registration and their referrals will not be viewable
* The evacuee will still be associated with an Evacuee Registration and the Evacuee’s addresses will remain in the database – we need to be careful not to physically delete registrations in the DB as there is no referential integrity check with the volunteers

### Volunteer/User
* The user will no longer appear in "User List" of an organization, when clicking through the Organization page "View/Add Users" button
* The user will not be returned in the "User List" search
* The user will no longer be viewable thus not editable
* The user will still be associated with an Organization as well as remain linked to any Registrations, which were finalized by the user.

### Organization
* An organization will not appear in the "List of Organizations & Users" and will not be searchable
* The organization will not be viewable nor editable
* Active Users belonging to a Soft Deleted organization will not be viewable or editable
* The users will still remain associated with the Organization

### Task
* An Incident Task will not appear in the "Task List" and will not be searchable
* The task will not be viewable nor editable
* The task will remain associated to any Evacuee registration

### Local Authority 
​​​​​​​Local authority is the org admin, they’re volunteers with a admin flag turned on and they can add, edit, view and deactivate volunteers in there org. There can be more than 1 admin, but only one primary contact for each org.

* The Local Authority would have the same consequences as Volunteer/User
* The Local Authority will still be able to Login