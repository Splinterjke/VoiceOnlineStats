var rgxp = /([^.:]+)/

function ClearTable() {
    var password = $("#password").val();
    fetch('/api/users/removeall', {
        headers: { 'Content-Type': 'application/json' },
        method: 'delete',
        body: JSON.stringify(password)
    }).then((response) => {
        if (response.status == 401) {
            alert("Неверный пароль");
            return;
        }

        if (response.status !== 200)
            return;
        location.reload();
    });
}

function GetTime(timespan) {
    if (timespan == null)
        return "Неизвестно";
    var timeArray = timespan.split(rgxp).filter(Boolean);
    var hours = 0;
    if (timeArray[1] == ".") {
        hours = parseInt(timeArray[0]) * 24 + parseInt(timeArray[2]);
        return `${hours} ч. ${timeArray[4]} мин. ${timeArray[6]} сек.`;
    }
    else {
        return `${timeArray[0]} ч. ${timeArray[2]} мин. ${timeArray[4]} сек.`;
    }
}

var $scroll = $('#scrollArea');
var $content = $('#contentArea');
var $headers = $("#headersArea");
$("#loader").show();
$("#statisticTable").hide();

function GetUsers() {
    fetch('/api/users', {
        headers: { 'Content-Type': 'application/json' },
        method: 'get'
    }).then((response) => {
        if (response.status !== 200)
            return;
        response.json().then((users) => {
            var data = [];
            $.each(users, function (index, user) {
                data.push(row(index + 1, user));
            });
            $("#loader").hide();
            $("#statisticTable").show();
            $("#members_count").append(users.length);
            var clusterize = new Clusterize({
                rows: data,
                rows_in_block: 20,
                blocks_in_cluster: 2,
                scrollId: 'scrollArea',
                contentId: 'contentArea',
                callbacks: {
                    clusterChanged: function () {
                        fitHeaderColumns();
                    }
                }
            });
            GetVoiceOnlineCount();
        });
    })
}

var fitHeaderColumns = (function () {
    var prevWidth = [];
    return function () {
        var $firstRow = $content.find('tr:not(.clusterize-extra-row):first');
        var columnsWidth = [];
        $firstRow.children().each(function () {
            columnsWidth.push($(this).width());
        });
        if (columnsWidth.toString() == prevWidth.toString()) return;
        $headers.find('tr').children().each(function (i) {
            if (i + 1 == $headers.find('tr').children().length)
                $(this).width(columnsWidth[i] + 20);
            else $(this).width(columnsWidth[i]);
        });
        prevWidth = columnsWidth;
    }
})();

function GetVoiceOnlineCount() {
    fetch('/api/users/voiceOnlineCount?type=current', {
        headers: { 'Content-Type': 'application/json' },
        method: 'get'
    }).then((response) => {
        if (response.status !== 200)
            return;
        response.json().then((count) => {
            $("#voicecurrentonline_count").append(count)
        });
    })

    fetch('/api/users/voiceOnlineCount?type=max', {
        headers: { 'Content-Type': 'application/json' },
        method: 'get'
    }).then((response) => {
        if (response.status !== 200)
            return;
        return response.json().then((count) => {
            $("#voicemaxonline_count").append(count);
        });
    })
}

var row = function (num, user) {
    var voiceOnlineTime = GetTime(user.voiceOnlineTime);
    var lastSessionTime = GetTime(user.lastSessionTime);
    var isInVoice = user.isConnected ? "Да" : "Нет";
    return `<tr data-rowid='"${num}'><td style='width:6%'>${num}</td><td style='width:15%'>${user.id}</td><td style='width:35%'>${user.username}</td><td style='width:17%'>${voiceOnlineTime}</td><td style='width:17%'>${lastSessionTime}</td><td style='width:10%'>${isInVoice}</td></tr>`;
}

GetUsers();