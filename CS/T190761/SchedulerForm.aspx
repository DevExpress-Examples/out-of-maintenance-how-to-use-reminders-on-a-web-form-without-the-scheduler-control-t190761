<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SchedulerForm.aspx.cs" Inherits="T190761.Default" %>

<%@ Register Assembly="DevExpress.Web.ASPxScheduler.v15.1, Version=15.1.15.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxScheduler" TagPrefix="dxwschs" %>
<%@ Register Assembly="DevExpress.XtraScheduler.v15.1.Core, Version=15.1.15.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraScheduler" TagPrefix="cc1" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.15.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" NavigateUrl="~/StorageForm.aspx" Text="Go to another page" />
            <dxwschs:ASPxScheduler ID="ASPxScheduler1" runat="server" ClientInstanceName="scheduler" ActiveViewType="WorkWeek" AppointmentDataSourceID="appointmentsDataSource" ClientIDMode="AutoID" ResourceDataSourceID="resourcesDataSource" Start="2014-12-22">
                <Storage>
                    <Appointments AutoRetrieveId="True">
                        <Mappings AllDay="AllDay" AppointmentId="UniqueId" Description="Description" End="EndDate" Label="Label" Location="Location" RecurrenceInfo="RecurrenceInfo" ReminderInfo="ReminderInfo" ResourceId="ResourceId" Start="StartDate" Status="Status" Subject="Subject" Type="Type" />
                    </Appointments>
                    <Resources>
                        <Mappings Caption="Description" Color="Color" ResourceId="Id" />
                    </Resources>
                </Storage>
                <Views>
                    <DayView>
                        <TimeRulers>
                            <cc1:TimeRuler></cc1:TimeRuler>
                        </TimeRulers>
                    </DayView>

                    <WorkWeekView>
                        <TimeRulers>
                            <cc1:TimeRuler></cc1:TimeRuler>
                        </TimeRulers>
                    </WorkWeekView>

                    <WeekView Enabled="false">
                    </WeekView>
                    <FullWeekView Enabled="true">
                        <TimeRulers>
                            <cc1:TimeRuler></cc1:TimeRuler>
                        </TimeRulers>
                    </FullWeekView>
                </Views>
            </dxwschs:ASPxScheduler>
            <asp:SqlDataSource ID="resourcesDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ScheduleTestConnectionString %>" SelectCommand="SELECT [Id], [Description], [Color] FROM [Resources]"></asp:SqlDataSource>
            <asp:SqlDataSource ID="appointmentsDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ScheduleTestConnectionString %>" DeleteCommand="DELETE FROM [Appointments] WHERE [UniqueId] = @UniqueId" InsertCommand="INSERT INTO [Appointments] ([Type], [StartDate], [EndDate], [AllDay], [Subject], [Location], [Description], [Status], [Label], [ResourceId], [ResourceIds], [ReminderInfo], [RecurrenceInfo], [PercentComplete], [CustomField1]) VALUES (@Type, @StartDate, @EndDate, @AllDay, @Subject, @Location, @Description, @Status, @Label, @ResourceId, @ResourceIds, @ReminderInfo, @RecurrenceInfo, @PercentComplete, @CustomField1)" SelectCommand="SELECT * FROM [Appointments]" UpdateCommand="UPDATE [Appointments] SET [Type] = @Type, [StartDate] = @StartDate, [EndDate] = @EndDate, [AllDay] = @AllDay, [Subject] = @Subject, [Location] = @Location, [Description] = @Description, [Status] = @Status, [Label] = @Label, [ResourceId] = @ResourceId, [ResourceIds] = @ResourceIds, [ReminderInfo] = @ReminderInfo, [RecurrenceInfo] = @RecurrenceInfo, [PercentComplete] = @PercentComplete, [CustomField1] = @CustomField1 WHERE [UniqueId] = @UniqueId">
                <DeleteParameters>
                    <asp:Parameter Name="UniqueId" Type="Int32" />
                </DeleteParameters>
                <InsertParameters>
                    <asp:Parameter Name="Type" Type="Int32" />
                    <asp:Parameter Name="StartDate" Type="DateTime" />
                    <asp:Parameter Name="EndDate" Type="DateTime" />
                    <asp:Parameter Name="AllDay" Type="Boolean" />
                    <asp:Parameter Name="Subject" Type="String" />
                    <asp:Parameter Name="Location" Type="String" />
                    <asp:Parameter Name="Description" Type="String" />
                    <asp:Parameter Name="Status" Type="Int32" />
                    <asp:Parameter Name="Label" Type="Int32" />
                    <asp:Parameter Name="ResourceId" Type="Int32" />
                    <asp:Parameter Name="ResourceIds" Type="String" />
                    <asp:Parameter Name="ReminderInfo" Type="String" />
                    <asp:Parameter Name="RecurrenceInfo" Type="String" />
                    <asp:Parameter Name="PercentComplete" Type="Int32" />
                    <asp:Parameter Name="CustomField1" Type="String" />
                </InsertParameters>
                <UpdateParameters>
                    <asp:Parameter Name="Type" Type="Int32" />
                    <asp:Parameter Name="StartDate" Type="DateTime" />
                    <asp:Parameter Name="EndDate" Type="DateTime" />
                    <asp:Parameter Name="AllDay" Type="Boolean" />
                    <asp:Parameter Name="Subject" Type="String" />
                    <asp:Parameter Name="Location" Type="String" />
                    <asp:Parameter Name="Description" Type="String" />
                    <asp:Parameter Name="Status" Type="Int32" />
                    <asp:Parameter Name="Label" Type="Int32" />
                    <asp:Parameter Name="ResourceId" Type="Int32" />
                    <asp:Parameter Name="ResourceIds" Type="String" />
                    <asp:Parameter Name="ReminderInfo" Type="String" />
                    <asp:Parameter Name="RecurrenceInfo" Type="String" />
                    <asp:Parameter Name="PercentComplete" Type="Int32" />
                    <asp:Parameter Name="CustomField1" Type="String" />
                    <asp:Parameter Name="UniqueId" Type="Int32" />
                </UpdateParameters>
            </asp:SqlDataSource>
        </div>
    </form>
</body>
</html>
