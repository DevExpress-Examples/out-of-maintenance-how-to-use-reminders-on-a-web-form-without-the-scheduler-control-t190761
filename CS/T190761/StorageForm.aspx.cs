using DevExpress.XtraScheduler;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace T190761 {
    public partial class WebForm1 : System.Web.UI.Page {
        SchedulerStorage storage;
        DataSet schedulerDataSet;
        SqlDataAdapter appointmentsAdapter;
        SqlDataAdapter resourceAdapter;
        SqlConnection connection;


        protected void Page_Init(object sender, EventArgs e) {
            storage = new SchedulerStorage();
            storage.ReminderAlert += storage_ReminderAlert;
        }

        protected void Page_Load(object sender, EventArgs e) {
            bool locked = false;
            schedulerDataSet = new DataSet();
            FillDataSet();
            MapAppointment(storage.Appointments.Mappings);
            MapResource(storage.Resources.Mappings);
            storage.Appointments.DataSource = schedulerDataSet.Tables["Appointments"];
            storage.Resources.DataSource = schedulerDataSet.Tables["Resources"];
            if(Session["AppointmentsCreated"] != null)
                locked = (bool)Session["AppointmentsCreated"];
            if(!IsPostBack && !IsCallback && !locked)
                for(int i = 0; i < 5; i++) { 
                    Appointment apt = storage.CreateAppointment(AppointmentType.Normal);
                    DateTime baseTime = DateTime.Now;
                    apt.Start = baseTime.AddMinutes(6);
                    apt.End = baseTime.AddHours(2);
                    apt.Subject = "TestReminder " + i;
                    Reminder reminder = apt.CreateNewReminder();
                    reminder.AlertTime = apt.Start;
                    reminder.TimeBeforeStart = new TimeSpan(0, 5, 0);
                    apt.Reminders.Add(reminder);
                    storage.Appointments.Add(apt);
                    appointmentsAdapter.Update(schedulerDataSet, "Appointments");
                    Session["AppointmentsCreated"] = true;
            }
                
        }

        protected void ASPxCallback1_Callback(object source, DevExpress.Web.CallbackEventArgs e) {
            if(e.Parameter != "") {
                int aptIdForDismiss = Convert.ToInt32(e.Parameter);
                foreach(Appointment apt in storage.Appointments.Items) {
                    if(((int)apt.Id) == aptIdForDismiss && apt.Reminder != null)
                        apt.Reminder.Dismiss();
                }
                appointmentsAdapter.Update(schedulerDataSet, "Appointments");
            }
            ASPxCallback1.JSProperties["cpReminders"] = null;
            storage.TriggerAlerts();
        }

        void storage_ReminderAlert(object sender, ReminderEventArgs e) {
            ReminderAlertNotificationCollection reminders = e.AlertNotifications;
            ASPxCallback1.JSProperties["cpReminders"] = reminders;
        }

        void MapResource(ResourceMappingInfo mappings) {
            mappings.Id = "Id";
            mappings.Caption = "Description";
        }

        void MapAppointment(AppointmentMappingInfo mappings) {
            mappings.AllDay = "AllDay";
            mappings.AppointmentId = "UniqueId";
            mappings.Description = "Description";
            mappings.End = "EndDate";
            mappings.Label = "Label";
            mappings.Location = "Location";
            mappings.RecurrenceInfo = "RecurrenceInfo";
            mappings.ReminderInfo = "ReminderInfo";
            mappings.ResourceId = "ResourceId";
            mappings.Start = "StartDate";
            mappings.Status = "Status";
            mappings.Subject = "Subject";
            mappings.Type = "Type";
        }

        void FillDataSet() {
            connection = new SqlConnection();
            connection.ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=SchedulerTest;Persist Security Info=True;User ID=sa;Password=dx";
            connection.Open();
            appointmentsAdapter = new SqlDataAdapter("SELECT * FROM [Appointments]", connection);

            #region UpdateCommand
            SqlCommand updateCommand = new SqlCommand();
            updateCommand.CommandText = 
                @"UPDATE [Appointments] SET [Type] = @Type, [StartDate] = @StartDate, [EndDate] = @EndDate, [AllDay] = @AllDay, "+
                @"[Subject] = @Subject, [Location] = @Location, [Description] = @Description, [Status] = @Status, [Label] = @Label, "+
                @"[ResourceId] = @ResourceId, [ResourceIds] = @ResourceIds, [ReminderInfo] = @ReminderInfo, [RecurrenceInfo] = @RecurrenceInfo "+
                @"WHERE (([UniqueId] = @Original_UniqueId) AND ((@IsNull_Type = 1 AND [Type] IS NULL) OR ([Type] = @Original_Type)) "+
                @"AND ((@IsNull_StartDate = 1 AND [StartDate] IS NULL) OR ([StartDate] = @Original_StartDate)) AND ((@IsNull_EndDate = 1 "+
                @"AND [EndDate] IS NULL) OR ([EndDate] = @Original_EndDate)) AND ((@IsNull_AllDay = 1 AND [AllDay] IS NULL) OR ([AllDay] = @Original_AllDay)) "+
                @"AND ((@IsNull_Status = 1 AND [Status] IS NULL) OR ([Status] = @Original_Status)) AND ((@IsNull_Label = 1 AND [Label] IS NULL) OR ([Label] = @Original_Label)) "+
                @"AND ((@IsNull_ResourceId = 1 AND [ResourceId] IS NULL) OR ([ResourceId] = @Original_ResourceId)) AND ((@IsNull_PercentComplete = 1 "+
                @"AND [PercentComplete] IS NULL) OR ([PercentComplete] = @Original_PercentComplete))); "+
                @"SELECT UniqueId, Type, StartDate, EndDate, AllDay, Subject, Location, Description, Status, Label, ResourceId, ResourceIds, ReminderInfo, "+
                @"RecurrenceInfo, PercentComplete, CustomField1 FROM Appointments WHERE (UniqueId = @UniqueId)";
            updateCommand.CommandType = CommandType.Text;
            updateCommand.Parameters.Add(new SqlParameter("@Type", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "Type", DataRowVersion.Current, false, null, "", "", ""));
            updateCommand.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.SmallDateTime, 0, ParameterDirection.Input, 0, 0, "StartDate", DataRowVersion.Current, false, null, "", "", ""));
            updateCommand.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.SmallDateTime, 0, ParameterDirection.Input, 0, 0, "EndDate", DataRowVersion.Current, false, null, "", "", ""));
            updateCommand.Parameters.Add(new SqlParameter("@AllDay", SqlDbType.Bit, 0, ParameterDirection.Input, 0, 0, "AllDay", DataRowVersion.Current, false, null, "", "", ""));
            updateCommand.Parameters.Add(new SqlParameter("@Subject", SqlDbType.NVarChar, 0, ParameterDirection.Input, 0, 0, "Subject", DataRowVersion.Current, false, null, "", "", ""));
            updateCommand.Parameters.Add(new SqlParameter("@Location", SqlDbType.NVarChar, 0, ParameterDirection.Input, 0, 0, "Location", DataRowVersion.Current, false, null, "", "", ""));
            updateCommand.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 0, ParameterDirection.Input, 0, 0, "Description", DataRowVersion.Current, false, null, "", "", ""));
            updateCommand.Parameters.Add(new SqlParameter("@Status", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "Status", DataRowVersion.Current, false, null, "", "", ""));
            updateCommand.Parameters.Add(new SqlParameter("@Label", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "Label", DataRowVersion.Current, false, null, "", "", ""));
            updateCommand.Parameters.Add(new SqlParameter("@ResourceId", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "ResourceId", DataRowVersion.Current, false, null, "", "", ""));
            updateCommand.Parameters.Add(new SqlParameter("@ResourceIds", SqlDbType.NVarChar, 0, ParameterDirection.Input, 0, 0, "ResourceIds", DataRowVersion.Current, false, null, "", "", ""));
            updateCommand.Parameters.Add(new SqlParameter("@ReminderInfo", SqlDbType.NVarChar, 0, ParameterDirection.Input, 0, 0, "ReminderInfo", DataRowVersion.Current, false, null, "", "", ""));
            updateCommand.Parameters.Add(new SqlParameter("@RecurrenceInfo", SqlDbType.NVarChar, 0, ParameterDirection.Input, 0, 0, "RecurrenceInfo", DataRowVersion.Current, false, null, "", "", ""));
            updateCommand.Parameters.Add(new SqlParameter("@Original_UniqueId", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "UniqueId", DataRowVersion.Original, false, null, "", "", ""));
            updateCommand.Parameters.Add(new SqlParameter("@IsNull_Type", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "Type", DataRowVersion.Original, true, null, "", "", ""));
            updateCommand.Parameters.Add(new SqlParameter("@Original_Type", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "Type", DataRowVersion.Original, false, null, "", "", ""));
            updateCommand.Parameters.Add(new SqlParameter("@IsNull_StartDate", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "StartDate", DataRowVersion.Original, true, null, "", "", ""));
            updateCommand.Parameters.Add(new SqlParameter("@Original_StartDate", SqlDbType.SmallDateTime, 0, ParameterDirection.Input, 0, 0, "StartDate", DataRowVersion.Original, false, null, "", "", ""));
            updateCommand.Parameters.Add(new SqlParameter("@IsNull_EndDate", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "EndDate", DataRowVersion.Original, true, null, "", "", ""));
            updateCommand.Parameters.Add(new SqlParameter("@Original_EndDate", SqlDbType.SmallDateTime, 0, ParameterDirection.Input, 0, 0, "EndDate", DataRowVersion.Original, false, null, "", "", ""));
            updateCommand.Parameters.Add(new SqlParameter("@IsNull_AllDay", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "AllDay", DataRowVersion.Original, true, null, "", "", ""));
            updateCommand.Parameters.Add(new SqlParameter("@Original_AllDay", SqlDbType.Bit, 0, ParameterDirection.Input, 0, 0, "AllDay", DataRowVersion.Original, false, null, "", "", ""));
            updateCommand.Parameters.Add(new SqlParameter("@IsNull_Status", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "Status", DataRowVersion.Original, true, null, "", "", ""));
            updateCommand.Parameters.Add(new SqlParameter("@Original_Status", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "Status", DataRowVersion.Original, false, null, "", "", ""));
            updateCommand.Parameters.Add(new SqlParameter("@IsNull_Label", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "Label", DataRowVersion.Original, true, null, "", "", ""));
            updateCommand.Parameters.Add(new SqlParameter("@Original_Label", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "Label", DataRowVersion.Original, false, null, "", "", ""));
            updateCommand.Parameters.Add(new SqlParameter("@IsNull_ResourceId", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "ResourceId", DataRowVersion.Original, true, null, "", "", ""));
            updateCommand.Parameters.Add(new SqlParameter("@Original_ResourceId", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "ResourceId", DataRowVersion.Original, false, null, "", "", ""));
            updateCommand.Parameters.Add(new SqlParameter("@IsNull_PercentComplete", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "PercentComplete", DataRowVersion.Original, true, null, "", "", ""));
            updateCommand.Parameters.Add(new SqlParameter("@Original_PercentComplete", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "PercentComplete", DataRowVersion.Original, false, null, "", "", ""));
            updateCommand.Parameters.Add(new SqlParameter("@UniqueId", SqlDbType.Int, 4, ParameterDirection.Input, 0, 0, "UniqueId", DataRowVersion.Current, false, null, "", "", ""));
            appointmentsAdapter.UpdateCommand = updateCommand;
            appointmentsAdapter.UpdateCommand.Connection = connection;
            #endregion

            #region InsertCommand
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.CommandText =
                "INSERT INTO [Appointments] ([Type], [StartDate], [EndDate], [AllDay], [Subject], [Location], " +
                "[Description], [Status], [Label], [ResourceId], [ResourceIds], [ReminderInfo], [RecurrenceInfo]) " +
                "VALUES (@Type, @StartDate, @EndDate, @AllDay, @Subject, @Location, @Description, @Status, @Label, " +
                "@ResourceId, @ResourceIds, @ReminderInfo, @RecurrenceInfo)";
            insertCommand.CommandType = CommandType.Text;
            insertCommand.Parameters.Add(new SqlParameter("@Type", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "Type", DataRowVersion.Current, false, null, "", "", ""));
            insertCommand.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.SmallDateTime, 0, ParameterDirection.Input, 0, 0, "StartDate", DataRowVersion.Current, false, null, "", "", ""));
            insertCommand.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.SmallDateTime, 0, ParameterDirection.Input, 0, 0, "EndDate", DataRowVersion.Current, false, null, "", "", ""));
            insertCommand.Parameters.Add(new SqlParameter("@AllDay", SqlDbType.Bit, 0, ParameterDirection.Input, 0, 0, "AllDay", DataRowVersion.Current, false, null, "", "", ""));
            insertCommand.Parameters.Add(new SqlParameter("@Subject", SqlDbType.NVarChar, 0, ParameterDirection.Input, 0, 0, "Subject", DataRowVersion.Current, false, null, "", "", ""));
            insertCommand.Parameters.Add(new SqlParameter("@Location", SqlDbType.NVarChar, 0, ParameterDirection.Input, 0, 0, "Location", DataRowVersion.Current, false, null, "", "", ""));
            insertCommand.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 0, ParameterDirection.Input, 0, 0, "Description", DataRowVersion.Current, false, null, "", "", ""));
            insertCommand.Parameters.Add(new SqlParameter("@Status", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "Status", DataRowVersion.Current, false, null, "", "", ""));
            insertCommand.Parameters.Add(new SqlParameter("@Label", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "Label", DataRowVersion.Current, false, null, "", "", ""));
            insertCommand.Parameters.Add(new SqlParameter("@ResourceId", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "ResourceId", DataRowVersion.Current, false, null, "", "", ""));
            insertCommand.Parameters.Add(new SqlParameter("@ResourceIds", SqlDbType.NVarChar, 0, ParameterDirection.Input, 0, 0, "ResourceIds", DataRowVersion.Current, false, null, "", "", ""));
            insertCommand.Parameters.Add(new SqlParameter("@ReminderInfo", SqlDbType.NVarChar, 0, ParameterDirection.Input, 0, 0, "ReminderInfo", DataRowVersion.Current, false, null, "", "", ""));
            insertCommand.Parameters.Add(new SqlParameter("@RecurrenceInfo", SqlDbType.NVarChar, 0, ParameterDirection.Input, 0, 0, "RecurrenceInfo", DataRowVersion.Current, false, null, "", "", ""));
            appointmentsAdapter.InsertCommand = insertCommand;
            appointmentsAdapter.InsertCommand.Connection = connection;
            #endregion

            resourceAdapter = new SqlDataAdapter("SELECT [Id], [Description] FROM [Resources]", connection);

            appointmentsAdapter.Fill(schedulerDataSet, "Appointments");
            resourceAdapter.Fill(schedulerDataSet, "Resources");
        }
    }
}