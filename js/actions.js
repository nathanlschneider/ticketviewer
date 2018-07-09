

    function checkAuth() {
        HEATAPI.Auth.getSignInfo(function (data) {
            if (data.authenticated) {
                document.getElementById('logButton').innerText = "Logout";
                document.getElementById('userInfo').innerText = "Logged in as " + data.login;
                api.callName();
            } else {
                document.getElementById('logButton').innerText = "Please Login";
                document.getElementById('userInfo').innerText = "";
            }
        });
    }

    function doLog() {
        var a = document.getElementById('logButton').innerText;
        if (a === 'Please Login') {
            HEATAPI.Auth.signIn(function (data) {
                console.log(data);
            })
        } else {
            HEATAPI.Auth.signOut(function (data) {
                alert("You have been logged out.");
                location.reload();
            });
        }
    };


    function contactCallback(dataArray) {

        for (i = 0; dataArray.data[i] !== undefined; i++) {
            ns.choices.push(dataArray.data[i].DisplayName);
            //console.log(dataArray.data[i].DisplayName);
        }
    }

        function processResult() {
            ticketCount = 0;
            var c = document.getElementById('content');
            var toAdd = document.createDocumentFragment();
            for (var i = 0; i < ns.apiData.data.length; i++) {
                if (custName == ns.apiData.data[i].ProfileFullName) {

                    ++ticketCount;
                    var newDiv = document.createElement('div');
                    var divAtt = document.createAttribute('onclick');
                    var classAtt = document.createAttribute('class');
                    divAtt.value = "showTicket(\"" + ns.apiData.data[i].Subject + "\",\"" + strip(ns.apiData.data[i].Symptom) + "\")";
                    classAtt.value = "incidentBox";
                    newDiv.setAttributeNode(classAtt);
                    newDiv.setAttributeNode(divAtt);
                    newDiv.innerHTML = "<span id='rarrow' class='glyphicon glyphicon-menu-right'></span><b>Incident:</b> #" +
                        ns.apiData.data[i].IncidentNumber + "<br><b>Priority: </b> " +
                        ns.apiData.data[i].Priority + "<br><b>Status: </b>" +
                        ns.apiData.data[i].Status + "<br><b>Subject: </b>" +
                        ts(ns.apiData.data[i].Subject);
                    toAdd.appendChild(newDiv);
                }
                c.appendChild(toAdd);
            }
            console.log(ticketCount);
            if (ticketCount === 0) {
                alert("This customer has no active incidents");
            }
        }
    
    function showTicket() {
        hideNodes();
        console.log(ns.ticketCount);
        var c = document.getElementById('ticket');
        c.style.display = "block";
        var toAdd = document.createDocumentFragment();
        var newDiv = document.createElement('div');
        newDiv.id = 'ticketContents';
        newDiv.innerHTML = "<p><b>Subject:</b><br>" + ns.apiData.data[ns.ticketCount].Subject + "</p><p><b>Description:</b><br>" + strip(ns.apiData.data[ns.ticketCount].Symptom) +
            "</p><div class='btn-group' role='group'><button type='button' id='backButton' onclick='hideTicket()' class='btn btn-default'><span class='glyphicon glyphicon-chevron-left'></span> Back</button></div>";
        toAdd.appendChild(newDiv);
        c.appendChild(toAdd);
    }

    function hideTicket() {
        showNodes();
        var c = document.getElementById('ticket');
        c.style.display = "none";
        var tc = document.getElementById('ticketContents');
        tc.remove();
    }

    function clearNodes() {
        var myNode = document.getElementById("content");
        myNode.innerHTML = '';
    }

    function hideNodes() {
        var parent = document.getElementById('content');
        if (parent.hasChildNodes) {
            var kids = parent.childNodes;
            for (var c = 0; c < kids.length; c++) {
                if (kids[c].style) {
                    kids[c].style.display = 'none';
                }
            }
        }
    }

    function showNodes() {
        var parent = document.getElementById('content');
        if (parent.hasChildNodes) {
            var kids = parent.childNodes;
            for (var c = 0; c < kids.length; c++) {
                if (kids[c].style) {
                    kids[c].style.display = 'block';
                }
            }
        }
    }

    function ts(string) {
        var length = 35;
        var trimmedString = string.length > length ?
            string.substring(0, length - 3) + "..." :
            string;
        return trimmedString;
    }

    function strip(html) {
        var doc = new DOMParser().parseFromString(html, 'text/html');
        var clean = doc.body.textContent;
        clean = clean.replace(/(\r\n\t|\n|\r\t)/gm, "");
        return clean || "";
    }


/*
function () {
    HEATAPIUtils.getJSONP(identityProviderUrl + '/Account/UserInfo', function (response) {
        if (!response.authenticated) {
            HEATAPI.Auth.signIn();
        } else {
            document.getElementById('logoutButton').innerText = "Logout";
            getList();
        }
    });
}

function getList() {
    var xmlhttp = new XMLHttpRequest();
    xmlhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            var data = JSON.parse(this.responseText);
            for (i = 0; data[i] !== undefined; i++) {
                choices.push(data[i].DisplayName);
            }
        }
    }
    xmlhttp.open("GET", "data/list.json", true);
    xmlhttp.send();
};

*/
