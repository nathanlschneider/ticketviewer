var api = (function () {
    //private
    var ticketpayLoadData = {};
    var namepayLoadData = {};
    var i = 0;
    var c = 0;
    var ticketObj = {};

    //creates 
    function Ticket(Category, CreatedDateTime, IncidentNumber, Owner, Priority, ProfileFullName, Status, Subject, Symptom) {
        this.Category = Category;
        this.CreatedDateTime = CreatedDateTime;
        this.IncidentNumber = IncidentNumber;
        this.Owner = Owner;
        this.Priority = Priority;
        this.ProfileFullName = ProfileFullName;
        this.Status = Status;
        this.Subject = Subject;
        this.Symptom = Symptom;
    }

    function callNameData() {
        HEATAPI.Search.SearchBusinessObject("Frs_CompositeContract_Contact", '',
            "DisplayName",
            "LastModDateTime",
            "DESC",
            500,
            function (payLoad) {

                if (payLoad.success) {
                    console.log("Name payLoad Receieved");
                } else {
                    console.log("Name payLoad Failure.");
                }

                for (i = 0; i < payLoad.data.length; i++) {
                    ns.choices[i] = payLoad.data[i].DisplayName;
                }
            });
        callTicketData();
    }

    function callTicketData() {
        var columnArray = ["Category",
            "CreatedDateTime",
            "IncidentNumber",
            "Owner",
            "Priority",
            "ProfileFullName",
            "Status",
            "Subject",
            "Symptom"
        ];
        HEATAPI.Search.SearchBusinessObject("Incident",
            '055c989e-d6ff-4deb-937a-3215dc26db51',
            columnArray,
            "LastModDateTime",
            "DESC",
            5000,
            function (payLoad) {
                ticketpayLoadData = payLoad;
                if (payLoad.success) {
                    console.log("Ticket Data payLoad Receieved");
                    for (c = 0; c < payLoad.data.length; c++) {
                        ticketObj[i] = new Ticket(payLoad.data[c].Category, payLoad.data[c].CreatedDateTime, payLoad.data[c].IncidentNumber,
                            payLoad.data[c].Priority, payLoad.data[c].ProfileFullName, payLoad.data[c].Status, payLoad.data[c].Symptom);
                    }
                } else {
                    console.log("Ticket payLoad Failure.");
                }
            })
    }

    function getNameData() {
        return namepayLoadData;
    }

    function getTicketData() {
        return ticketpayLoadData;
    }

    function test(arg) {
        return ticketObj[arg];
    }
    //public
    return {
        callName: callNameData,
        callData: callTicketData,
        name: getNameData,
        data: getTicketData,
        test: test
    }
})();
