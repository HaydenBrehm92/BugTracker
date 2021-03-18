$(document).ready(function () {
    window.timeList;   //global, window might now be the best idea but it works

    function getLocalTime() {
        var value = Intl.DateTimeFormat().resolvedOptions().timeZone;   //Client-side timezone https://stackoverflow.com/questions/9772955/how-can-i-get-the-timezone-name-in-javascript
        var projectid = $("#viewbagid").data("value");
        $.ajax({
            url: "/Projects/BugConvertUtcToLocal",
            type: "POST",
            data: '{"timezone": "' + value + '", "projectid": "'+ projectid +'"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                buildDateString(data);
                success(window.timeList);
            }

        });
    };

    function success(data) {
        AddDateToBugs();
        console.log("success");
    }

    function buildDateString(data) {
        var dataStr = JSON.stringify(data);
        window.timeList = JSON.parse(dataStr) //global json object array

        // Convert to Local Time and insert into Global timeList
        for (var key in window.timeList) {
            var currDateStr = window.timeList[key].CreatedDate;
            var currTimeStr = window.timeList[key].ModifiedDate;
            var localCreated = new Date(parseInt(currDateStr.replace('/Date(', ''))) //take /Date( out
            var localModified = new Date(parseInt(currTimeStr.replace('/Date(', '')))

            window.timeList[key].CreatedDate = localCreated;
            window.timeList[key].ModifiedDate = localModified;
            //console.log(timeList[key].id, timeList[key].CreatedDate, timeList[key].ModifiedDate);
        }
    }

    function AddDateToBugs() {
        //console.log("In AddDateToCells")
        for (var key in window.timeList) {
            var modDate = new Date(window.timeList[key].ModifiedDate).toLocaleDateString();
            var modTime = new Date(window.timeList[key].ModifiedDate).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit', hour12: true });
            var createdDate = new Date(window.timeList[key].CreatedDate).toLocaleDateString();
            var dateStr = $("#bugDates-" + JSON.stringify(window.timeList[key].id));
            var currBug = dateStr;
            var newModStr = modDate + " " + modTime;    //new string with date + time

            var pMod = document.createElement('p');
            var pCreate = document.createElement('p');

            
            pMod.style.fontSize = "10pt";
            pCreate.style.fontSize = "10pt";

            pMod.innerHTML = "Modified: " + newModStr;
            pMod.className = "date-format font-italic mr-auto mb-0 text-muted";
            pCreate.innerHTML = "Created: " + createdDate;
            pCreate.className = "date-format font-italic ml-auto mb-0 text-muted";

            currBug.append(pMod);
            currBug.append(pCreate);
        }
    }
    getLocalTime();
});
