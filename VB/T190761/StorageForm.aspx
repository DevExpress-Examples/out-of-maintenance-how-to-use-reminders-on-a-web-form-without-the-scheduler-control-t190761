<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="StorageForm.aspx.vb" Inherits="T190761.WebForm1" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.17.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<script type="text/javascript">
    var dismissedReminder = "";
    function OnTick() {
        callback.PerformCallback();
    }

    function OnDismissButtonClick() {
        var index = listBox.GetSelectedIndex();
        var reminders = callback.cpReminders;
        if (reminders != null && index >= 0) {
            var reminder = reminders[index];
            dismissedReminder = reminder.ActualAppointment.Id;
            callback.PerformCallback(dismissedReminder.toString());
        }
    }

    function OnBeginCallback() {
        timer.SetEnabled(false);
        listBox.SetEnabled(false);
    }

    function OnCallbackComplete() {
        ProcessReminders();
        timer.SetEnabled(true);
        listBox.SetEnabled(true);
    }

    function ProcessReminders() {
        listBox.ClearItems();
        var reminders = callback.cpReminders;
        if (reminders != null && reminders.length > 0)
            for (var i = 0; i < reminders.length; i++) {
                var reminder = reminders[i];
                var aptId = reminder.ActualAppointment.Id;
                var aptSubj = reminder.ActualAppointment.Subject;
                var aptStart = reminder.ActualAppointment.Start;
                var item = [aptId.toString(), aptStart.toString(), aptSubj.toString(), '<input type="button" onclick="OnDismissButtonClick();" value="Dismiss" />'];
                listBox.AddItem(item);
            }
    }
</script>
<body>
    <form id="form1" runat="server">
        <div>
            <dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" NavigateUrl="~/SchedulerForm.aspx" Text="Go to Scheduler page" />
            <dx:ASPxListBox ID="ASPxListBox1" runat="server" EncodeHtml="False" ClientInstanceName="listBox" Height="284px">
                <Columns>
                    <dx:ListBoxColumn Caption="ID" Name="IDColumn" />
                    <dx:ListBoxColumn Caption="StartDate" Name="StartDateColumn" Width="200px" />
                    <dx:ListBoxColumn Caption="Subject" Width="400px" />
                    <dx:ListBoxColumn Caption="Dismiss" Name="DismissButtonColumn" Width="70px" />
                </Columns>
            </dx:ASPxListBox>
            <dx:ASPxCallback ID="ASPxCallback1" runat="server" ClientInstanceName="callback" OnCallback="ASPxCallback1_Callback">
                <ClientSideEvents CallbackComplete="OnCallbackComplete" BeginCallback="OnBeginCallback" />
            </dx:ASPxCallback>
        </div>
        <dx:ASPxTimer ID="ASPxTimer1" runat="server" Interval="10000" ClientInstanceName="timer">
            <ClientSideEvents Tick="OnTick" />
        </dx:ASPxTimer>
    </form>
</body>
</html>