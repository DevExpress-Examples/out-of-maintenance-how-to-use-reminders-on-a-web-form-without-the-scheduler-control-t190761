<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128547818/14.2.6%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T190761)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
<!-- default file list -->
*Files to look at*:

* [Global.asax](./CS/T190761/Global.asax) (VB: [Global.asax](./VB/T190761/Global.asax))
* [Global.asax.cs](./CS/T190761/Global.asax.cs) (VB: [Global.asax.vb](./VB/T190761/Global.asax.vb))
* [SchedulerForm.aspx](./CS/T190761/SchedulerForm.aspx) (VB: [SchedulerForm.aspx](./VB/T190761/SchedulerForm.aspx))
* [SchedulerForm.aspx.cs](./CS/T190761/SchedulerForm.aspx.cs) (VB: [SchedulerForm.aspx.vb](./VB/T190761/SchedulerForm.aspx.vb))
* [StorageForm.aspx](./CS/T190761/StorageForm.aspx) (VB: [StorageForm.aspx](./VB/T190761/StorageForm.aspx))
* [StorageForm.aspx.cs](./CS/T190761/StorageForm.aspx.cs) (VB: [StorageForm.aspx.vb](./VB/T190761/StorageForm.aspx.vb))
<!-- default file list end -->
# How to use reminders on a web form without the scheduler control


<p>This example demonstrates how to work with reminders on a web page that does not have the ASPxScheduler control.</p>
<p>This approach uses the non-visual <a href="https://documentation.devexpress.com/#WindowsForms/clsDevExpressXtraSchedulerSchedulerStoragetopic">SchedulerStorage</a> component to obtain appointments from a database.<br />Every 10 seconds the timer initiates a callback to the <a href="https://documentation.devexpress.com/#AspNet/clsDevExpressWebASPxCallbacktopic">ASPxCallback</a> control with an empty parameter. In the server <a href="https://documentation.devexpress.com/#AspNet/DevExpressWebASPxCallback_Callbacktopic">ASPxCallback.Callback</a> event handler, we call the <br /><a href="https://documentation.devexpress.com/#CoreLibraries/DevExpressXtraSchedulerSchedulerStorageBase_TriggerAlertstopic">SchedulerStorage.TriggerAlerts</a> method that fires the <a href="https://documentation.devexpress.com/#CoreLibraries/DevExpressXtraSchedulerSchedulerStorageBase_ReminderAlerttopic">SchedulerStorage.ReminderAlert</a> event. In the <strong>SchedulerStorage.ReminderAlert</strong> event handler, we can obtain the <br /><a href="https://documentation.devexpress.com/#CoreLibraries/clsDevExpressXtraSchedulerReminderAlertNotificationCollectiontopic">ReminderAlertNotificationCollection</a> and pass it to the client via <a href="https://documentation.devexpress.com/#AspNet/DevExpressWebASPxCallback_JSPropertiestopic">ASPxCallback.JSProperties</a>.<br />In the <a href="https://documentation.devexpress.com/#AspNet/DevExpressWebScriptsASPxClientCallback_CallbackCompletetopic">ASPxClientCallback.CallbackComplete</a> event handler, we check the <strong>ReminderAlertNotificationCollection</strong> and add all the reminders to the ASPxListBox control.<br />When an end-user wants to dismiss a specific reminder, he/she must click the <strong>Dismiss</strong> button for a certain appointment.<br />Then we send a callback to the <strong>ASPxCallback</strong> control with the appointment Id as a parameter.<br />In the <strong>ASPxCallback.Callback</strong> event we use the <a href="https://documentation.devexpress.com/#CoreLibraries/DevExpressXtraSchedulerReminderBase_Dismisstopic">Reminder.Dismiss</a> method to dismiss the reminder.<br /><br />When the example starts, it creates five appointments with reminders. Wait for a couple of minutes to see the appointments in the ASPxListBox control.<br />Then you can use the Dismiss button to dismiss a certain reminder.</p>
<p>Use the <strong>sqript.sql</strong> script from the project to create a scheduler database. Make sure you change the connection string.</p>
<p><br /><strong>Note</strong>, to use this example, it is necessary to have both the ASP and WinForms subscriptions.</p>


<h3>Description</h3>

Hello,<br />This example demonstrates how to work with reminders on the web page that does not have the ASPxScheduler control.<br />This approach uses the non-visual <a href="https://documentation.devexpress.com/#WindowsForms/clsDevExpressXtraSchedulerSchedulerStoragetopic">SchedulerStorage</a>&nbsp;component to obtain the appointments from the database.

<br/>


