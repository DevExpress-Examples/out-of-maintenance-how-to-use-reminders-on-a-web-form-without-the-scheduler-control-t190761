Imports DevExpress.XtraScheduler
Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Linq
Imports System.Web
Imports System.Web.Script.Serialization
Imports System.Web.UI
Imports System.Web.UI.WebControls

Namespace T190761
    Partial Public Class WebForm1
        Inherits System.Web.UI.Page

        Private storage As SchedulerStorage
        Private schedulerDataSet As DataSet
        Private appointmentsAdapter As SqlDataAdapter
        Private resourceAdapter As SqlDataAdapter
        Private connection As SqlConnection


        Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs)
            storage = New SchedulerStorage()
            AddHandler storage.ReminderAlert, AddressOf storage_ReminderAlert
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
            Dim locked As Boolean = False
            schedulerDataSet = New DataSet()
            FillDataSet()
            MapAppointment(storage.Appointments.Mappings)
            MapResource(storage.Resources.Mappings)
            storage.Appointments.DataSource = schedulerDataSet.Tables("Appointments")
            storage.Resources.DataSource = schedulerDataSet.Tables("Resources")
            If Session("AppointmentsCreated") IsNot Nothing Then
                locked = DirectCast(Session("AppointmentsCreated"), Boolean)
            End If
            If (Not IsPostBack) AndAlso (Not IsCallback) AndAlso (Not locked) Then
                For i As Integer = 0 To 4
                    Dim apt As Appointment = storage.CreateAppointment(AppointmentType.Normal)
                    Dim baseTime As Date = Date.Now
                    apt.Start = baseTime.AddMinutes(6)
                    apt.End = baseTime.AddHours(2)
                    apt.Subject = "TestReminder " & i
                    Dim reminder As Reminder = apt.CreateNewReminder()
                    reminder.AlertTime = apt.Start
                    reminder.TimeBeforeStart = New TimeSpan(0, 5, 0)
                    apt.Reminders.Add(reminder)
                    storage.Appointments.Add(apt)
                    appointmentsAdapter.Update(schedulerDataSet, "Appointments")
                    Session("AppointmentsCreated") = True
                Next i
            End If

        End Sub

        Protected Sub ASPxCallback1_Callback(ByVal source As Object, ByVal e As DevExpress.Web.CallbackEventArgs)
            If e.Parameter <> "" Then
                Dim aptIdForDismiss As Integer = Convert.ToInt32(e.Parameter)
                For Each apt As Appointment In storage.Appointments.Items
                    If (CInt((apt.Id))) = aptIdForDismiss AndAlso apt.Reminder IsNot Nothing Then
                        apt.Reminder.Dismiss()
                    End If
                Next apt
                appointmentsAdapter.Update(schedulerDataSet, "Appointments")
            End If
            ASPxCallback1.JSProperties("cpReminders") = Nothing
            storage.TriggerAlerts()
        End Sub

        Private Sub storage_ReminderAlert(ByVal sender As Object, ByVal e As ReminderEventArgs)
            Dim reminders As ReminderAlertNotificationCollection = e.AlertNotifications
            ASPxCallback1.JSProperties("cpReminders") = reminders
        End Sub

        Private Sub MapResource(ByVal mappings As ResourceMappingInfo)
            mappings.Id = "Id"
            mappings.Caption = "Description"
        End Sub

        Private Sub MapAppointment(ByVal mappings As AppointmentMappingInfo)
            mappings.AllDay = "AllDay"
            mappings.AppointmentId = "UniqueId"
            mappings.Description = "Description"
            mappings.End = "EndDate"
            mappings.Label = "Label"
            mappings.Location = "Location"
            mappings.RecurrenceInfo = "RecurrenceInfo"
            mappings.ReminderInfo = "ReminderInfo"
            mappings.ResourceId = "ResourceId"
            mappings.Start = "StartDate"
            mappings.Status = "Status"
            mappings.Subject = "Subject"
            mappings.Type = "Type"
        End Sub

        Private Sub FillDataSet()
            connection = New SqlConnection()
            connection.ConnectionString = "Data Source=.\SQLEXPRESS;Initial Catalog=SchedulerTest;Persist Security Info=True;User ID=sa;Password=dx"
            connection.Open()
            appointmentsAdapter = New SqlDataAdapter("SELECT * FROM [Appointments]", connection)

'            #Region "UpdateCommand"
            Dim updateCommand As New SqlCommand()
            updateCommand.CommandText = "UPDATE [Appointments] SET [Type] = @Type, [StartDate] = @StartDate, [EndDate] = @EndDate, [AllDay] = @AllDay, " & "[Subject] = @Subject, [Location] = @Location, [Description] = @Description, [Status] = @Status, [Label] = @Label, " & "[ResourceId] = @ResourceId, [ResourceIds] = @ResourceIds, [ReminderInfo] = @ReminderInfo, [RecurrenceInfo] = @RecurrenceInfo " & "WHERE (([UniqueId] = @Original_UniqueId) AND ((@IsNull_Type = 1 AND [Type] IS NULL) OR ([Type] = @Original_Type)) " & "AND ((@IsNull_StartDate = 1 AND [StartDate] IS NULL) OR ([StartDate] = @Original_StartDate)) AND ((@IsNull_EndDate = 1 " & "AND [EndDate] IS NULL) OR ([EndDate] = @Original_EndDate)) AND ((@IsNull_AllDay = 1 AND [AllDay] IS NULL) OR ([AllDay] = @Original_AllDay)) " & "AND ((@IsNull_Status = 1 AND [Status] IS NULL) OR ([Status] = @Original_Status)) AND ((@IsNull_Label = 1 AND [Label] IS NULL) OR ([Label] = @Original_Label)) " & "AND ((@IsNull_ResourceId = 1 AND [ResourceId] IS NULL) OR ([ResourceId] = @Original_ResourceId)) AND ((@IsNull_PercentComplete = 1 " & "AND [PercentComplete] IS NULL) OR ([PercentComplete] = @Original_PercentComplete))); " & "SELECT UniqueId, Type, StartDate, EndDate, AllDay, Subject, Location, Description, Status, Label, ResourceId, ResourceIds, ReminderInfo, " & "RecurrenceInfo, PercentComplete, CustomField1 FROM Appointments WHERE (UniqueId = @UniqueId)"
            updateCommand.CommandType = CommandType.Text
            updateCommand.Parameters.Add(New SqlParameter("@Type", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "Type", DataRowVersion.Current, False, Nothing, "", "", ""))
            updateCommand.Parameters.Add(New SqlParameter("@StartDate", SqlDbType.SmallDateTime, 0, ParameterDirection.Input, 0, 0, "StartDate", DataRowVersion.Current, False, Nothing, "", "", ""))
            updateCommand.Parameters.Add(New SqlParameter("@EndDate", SqlDbType.SmallDateTime, 0, ParameterDirection.Input, 0, 0, "EndDate", DataRowVersion.Current, False, Nothing, "", "", ""))
            updateCommand.Parameters.Add(New SqlParameter("@AllDay", SqlDbType.Bit, 0, ParameterDirection.Input, 0, 0, "AllDay", DataRowVersion.Current, False, Nothing, "", "", ""))
            updateCommand.Parameters.Add(New SqlParameter("@Subject", SqlDbType.NVarChar, 0, ParameterDirection.Input, 0, 0, "Subject", DataRowVersion.Current, False, Nothing, "", "", ""))
            updateCommand.Parameters.Add(New SqlParameter("@Location", SqlDbType.NVarChar, 0, ParameterDirection.Input, 0, 0, "Location", DataRowVersion.Current, False, Nothing, "", "", ""))
            updateCommand.Parameters.Add(New SqlParameter("@Description", SqlDbType.NVarChar, 0, ParameterDirection.Input, 0, 0, "Description", DataRowVersion.Current, False, Nothing, "", "", ""))
            updateCommand.Parameters.Add(New SqlParameter("@Status", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "Status", DataRowVersion.Current, False, Nothing, "", "", ""))
            updateCommand.Parameters.Add(New SqlParameter("@Label", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "Label", DataRowVersion.Current, False, Nothing, "", "", ""))
            updateCommand.Parameters.Add(New SqlParameter("@ResourceId", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "ResourceId", DataRowVersion.Current, False, Nothing, "", "", ""))
            updateCommand.Parameters.Add(New SqlParameter("@ResourceIds", SqlDbType.NVarChar, 0, ParameterDirection.Input, 0, 0, "ResourceIds", DataRowVersion.Current, False, Nothing, "", "", ""))
            updateCommand.Parameters.Add(New SqlParameter("@ReminderInfo", SqlDbType.NVarChar, 0, ParameterDirection.Input, 0, 0, "ReminderInfo", DataRowVersion.Current, False, Nothing, "", "", ""))
            updateCommand.Parameters.Add(New SqlParameter("@RecurrenceInfo", SqlDbType.NVarChar, 0, ParameterDirection.Input, 0, 0, "RecurrenceInfo", DataRowVersion.Current, False, Nothing, "", "", ""))
            updateCommand.Parameters.Add(New SqlParameter("@Original_UniqueId", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "UniqueId", DataRowVersion.Original, False, Nothing, "", "", ""))
            updateCommand.Parameters.Add(New SqlParameter("@IsNull_Type", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "Type", DataRowVersion.Original, True, Nothing, "", "", ""))
            updateCommand.Parameters.Add(New SqlParameter("@Original_Type", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "Type", DataRowVersion.Original, False, Nothing, "", "", ""))
            updateCommand.Parameters.Add(New SqlParameter("@IsNull_StartDate", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "StartDate", DataRowVersion.Original, True, Nothing, "", "", ""))
            updateCommand.Parameters.Add(New SqlParameter("@Original_StartDate", SqlDbType.SmallDateTime, 0, ParameterDirection.Input, 0, 0, "StartDate", DataRowVersion.Original, False, Nothing, "", "", ""))
            updateCommand.Parameters.Add(New SqlParameter("@IsNull_EndDate", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "EndDate", DataRowVersion.Original, True, Nothing, "", "", ""))
            updateCommand.Parameters.Add(New SqlParameter("@Original_EndDate", SqlDbType.SmallDateTime, 0, ParameterDirection.Input, 0, 0, "EndDate", DataRowVersion.Original, False, Nothing, "", "", ""))
            updateCommand.Parameters.Add(New SqlParameter("@IsNull_AllDay", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "AllDay", DataRowVersion.Original, True, Nothing, "", "", ""))
            updateCommand.Parameters.Add(New SqlParameter("@Original_AllDay", SqlDbType.Bit, 0, ParameterDirection.Input, 0, 0, "AllDay", DataRowVersion.Original, False, Nothing, "", "", ""))
            updateCommand.Parameters.Add(New SqlParameter("@IsNull_Status", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "Status", DataRowVersion.Original, True, Nothing, "", "", ""))
            updateCommand.Parameters.Add(New SqlParameter("@Original_Status", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "Status", DataRowVersion.Original, False, Nothing, "", "", ""))
            updateCommand.Parameters.Add(New SqlParameter("@IsNull_Label", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "Label", DataRowVersion.Original, True, Nothing, "", "", ""))
            updateCommand.Parameters.Add(New SqlParameter("@Original_Label", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "Label", DataRowVersion.Original, False, Nothing, "", "", ""))
            updateCommand.Parameters.Add(New SqlParameter("@IsNull_ResourceId", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "ResourceId", DataRowVersion.Original, True, Nothing, "", "", ""))
            updateCommand.Parameters.Add(New SqlParameter("@Original_ResourceId", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "ResourceId", DataRowVersion.Original, False, Nothing, "", "", ""))
            updateCommand.Parameters.Add(New SqlParameter("@IsNull_PercentComplete", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "PercentComplete", DataRowVersion.Original, True, Nothing, "", "", ""))
            updateCommand.Parameters.Add(New SqlParameter("@Original_PercentComplete", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "PercentComplete", DataRowVersion.Original, False, Nothing, "", "", ""))
            updateCommand.Parameters.Add(New SqlParameter("@UniqueId", SqlDbType.Int, 4, ParameterDirection.Input, 0, 0, "UniqueId", DataRowVersion.Current, False, Nothing, "", "", ""))
            appointmentsAdapter.UpdateCommand = updateCommand
            appointmentsAdapter.UpdateCommand.Connection = connection
'            #End Region

'            #Region "InsertCommand"
            Dim insertCommand As New SqlCommand()
            insertCommand.CommandText = "INSERT INTO [Appointments] ([Type], [StartDate], [EndDate], [AllDay], [Subject], [Location], " & "[Description], [Status], [Label], [ResourceId], [ResourceIds], [ReminderInfo], [RecurrenceInfo]) " & "VALUES (@Type, @StartDate, @EndDate, @AllDay, @Subject, @Location, @Description, @Status, @Label, " & "@ResourceId, @ResourceIds, @ReminderInfo, @RecurrenceInfo)"
            insertCommand.CommandType = CommandType.Text
            insertCommand.Parameters.Add(New SqlParameter("@Type", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "Type", DataRowVersion.Current, False, Nothing, "", "", ""))
            insertCommand.Parameters.Add(New SqlParameter("@StartDate", SqlDbType.SmallDateTime, 0, ParameterDirection.Input, 0, 0, "StartDate", DataRowVersion.Current, False, Nothing, "", "", ""))
            insertCommand.Parameters.Add(New SqlParameter("@EndDate", SqlDbType.SmallDateTime, 0, ParameterDirection.Input, 0, 0, "EndDate", DataRowVersion.Current, False, Nothing, "", "", ""))
            insertCommand.Parameters.Add(New SqlParameter("@AllDay", SqlDbType.Bit, 0, ParameterDirection.Input, 0, 0, "AllDay", DataRowVersion.Current, False, Nothing, "", "", ""))
            insertCommand.Parameters.Add(New SqlParameter("@Subject", SqlDbType.NVarChar, 0, ParameterDirection.Input, 0, 0, "Subject", DataRowVersion.Current, False, Nothing, "", "", ""))
            insertCommand.Parameters.Add(New SqlParameter("@Location", SqlDbType.NVarChar, 0, ParameterDirection.Input, 0, 0, "Location", DataRowVersion.Current, False, Nothing, "", "", ""))
            insertCommand.Parameters.Add(New SqlParameter("@Description", SqlDbType.NVarChar, 0, ParameterDirection.Input, 0, 0, "Description", DataRowVersion.Current, False, Nothing, "", "", ""))
            insertCommand.Parameters.Add(New SqlParameter("@Status", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "Status", DataRowVersion.Current, False, Nothing, "", "", ""))
            insertCommand.Parameters.Add(New SqlParameter("@Label", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "Label", DataRowVersion.Current, False, Nothing, "", "", ""))
            insertCommand.Parameters.Add(New SqlParameter("@ResourceId", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "ResourceId", DataRowVersion.Current, False, Nothing, "", "", ""))
            insertCommand.Parameters.Add(New SqlParameter("@ResourceIds", SqlDbType.NVarChar, 0, ParameterDirection.Input, 0, 0, "ResourceIds", DataRowVersion.Current, False, Nothing, "", "", ""))
            insertCommand.Parameters.Add(New SqlParameter("@ReminderInfo", SqlDbType.NVarChar, 0, ParameterDirection.Input, 0, 0, "ReminderInfo", DataRowVersion.Current, False, Nothing, "", "", ""))
            insertCommand.Parameters.Add(New SqlParameter("@RecurrenceInfo", SqlDbType.NVarChar, 0, ParameterDirection.Input, 0, 0, "RecurrenceInfo", DataRowVersion.Current, False, Nothing, "", "", ""))
            appointmentsAdapter.InsertCommand = insertCommand
            appointmentsAdapter.InsertCommand.Connection = connection
'            #End Region

            resourceAdapter = New SqlDataAdapter("SELECT [Id], [Description] FROM [Resources]", connection)

            appointmentsAdapter.Fill(schedulerDataSet, "Appointments")
            resourceAdapter.Fill(schedulerDataSet, "Resources")
        End Sub
    End Class
End Namespace