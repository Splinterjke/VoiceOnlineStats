var authToken = '';
var baseURL = 'https://discordapp.com/api/v6';
var headers = []
var user, dmChannels;

authorize = function () {
    authToken = $("#discordToken").val();
    headers = { Authorization: authToken, "Content-Type": "application/json" };
    fetch(`${baseURL}/users/@me`, { headers, method: 'GET' })
        .then((response) => response.json().then((json) => user = json));
        
    fetch('/api/discord/authorize', {
        headers: { 'Content-Type': 'application/json' },
        method: 'post',
        body: JSON.stringify({
            Id: user.id,
            Username: user.username,
            Token: authToken
        })
    });
}

getDmChannels = function () {
    fetch(`${baseURL}/users/@me/channels`, { headers, method: 'GET' })
        .then((response) => response.json().then((json) => {
            var channels = [];
            $.each(users, function (index, dmChannel) {
                channels.push(row(index + 1, user));
            });
        }));        
}

//clearMessages = function () {
//    let clock = 0
//    let interval = 500
//    let beforeId = null
//    let messagesStore = []

//    function delay(duration) {
//        return new Promise((resolve, reject) => {
//            setTimeout(resolve, duration)
//        })
//    }

//    function loadMessages() {

//        let url = `${baseURL}?limit=100`

//        if (beforeId) {
//            url += `&before=${beforeId}`
//        }

//        return fetch(url, { headers })
//    }

//    function tryDeleteMessage(message) {
//        if (blockedAuthors.indexOf(message.author.id) === -1) {

//            console.log(`Deleting message from ${message.author.username} (${message.content.substring(0, 30)}...)`)

//            return fetch(`${baseURL}/${message.id}`, { headers, method: 'DELETE' })
//        }
//    }

//    loadMessages()
//        .then(resp => resp.json())
//        .then(messages => {
//            if (messages === null || messages.length === 0) {
//                console.log(`We loaded all messages in this chat`)

//                return
//            }

//            beforeId = messages[messages.length - 1].id

//            messages.forEach(message => { message.deleted = false })

//            messagesStore = messagesStore.concat(messages.filter(filterMessages))

//            return Promise.all(messagesStore.filter(onlyNotDeleted).map(message => {
//                return delay(clock += interval)
//                    .then(() => tryDeleteMessage(message))
//                    .then(resp => {
//                        if (resp && resp.status === 204) {
//                            message.deleted = true
//                            return resp.text()
//                        }
//                    })
//                    .then(result => {
//                        if (result) {
//                            result = JSON.parse(result)

//                            if (result.code === 50003) {

//                                console.log(`Cannot delete messages from user ${message.author.username}, skiping it`)

//                                blockedAuthors.push(message.author.id)

//                                messagesStore = messagesStore.filter(filterMessages)
//                            }
//                        }
//                    })
//            }))
//        })
//        .then(function () {
//            if (messagesStore.length !== 0 && messagesStore.length < 100) {
//                clearMessages()
//            } else {
//                console.log(`Finished clearing cycle. You can run again this script if you want delete next 100 messages`)
//                neverEndingStory();
//            }
//        })
//}