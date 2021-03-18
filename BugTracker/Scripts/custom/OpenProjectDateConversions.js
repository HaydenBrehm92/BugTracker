$(document).ready(function () {
    window.timeList;   //global, window might now be the best idea but it works

    function getLocalTime() {
        var value = Intl.DateTimeFormat().resolvedOptions().timeZone;   //Client-side timezone https://stackoverflow.com/questions/9772955/how-can-i-get-the-timezone-name-in-javascript

        $.ajax({
            url: "/Projects/OpenProjectConvertUtcToLocal",
            type: "POST",
            data: '{"timezone": "' + value + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                buildDateString(data);
                success(window.timeList);
            }

        });
    };

    function success(data) {
        AddDateToCells();
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

    function AddDateToCells() {
        //console.log("In AddDateToCells")
        for (var key in window.timeList) {
            var modDate = new Date(window.timeList[key].ModifiedDate).toLocaleDateString();
            var modTime = new Date(window.timeList[key].ModifiedDate).toLocaleTimeString([], {hour: '2-digit', minute: '2-digit', hour12: true});
            var createdDate = new Date(window.timeList[key].CreatedDate).toLocaleDateString();
            var modStr = $("#modifiedTD-" + JSON.stringify(window.timeList[key].id));
            var createStr = $("#createdTD-" + JSON.stringify(window.timeList[key].id));
            var currModCell = modStr;
            var currDateCell = createStr;
            var pMod1 = document.createElement('p');
            var pMod2 = document.createElement('p');
            var pCreate = document.createElement('p');
            pMod1.style.fontSize = "11pt";
            pMod2.style.fontSize = "11pt";
            pCreate.style.fontSize = "11pt";
            pMod1.innerHTML = modDate;
            pMod1.className = "m-0";
            pMod2.innerHTML = modTime;
            pMod2.className = "m-0";
            pCreate.innerHTML = createdDate;
            pCreate.className = "m-0";
            currModCell.append(pMod1);
            currModCell.append(pMod2);
            currDateCell.append(pCreate);
        }
    }
    getLocalTime();
});
