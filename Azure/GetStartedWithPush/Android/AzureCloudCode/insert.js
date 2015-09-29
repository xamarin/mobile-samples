function insert(item, user, request) 
{
    console.log('todoitem.Insert() fired');        
    
    if (item.text.length > 10) {
        request.respond(statusCodes.BAD_REQUEST, 'Text length must be 10 characters or less.');        
    } 
    else {    
        // item.createdAt = new Date();
        // item.userId = user.userId; 
        // request.execute();
               
        // ANDROID - 
        if (item.channel) {
            console.log('todoitem.Insert() - channel found on item');
            request.execute({
                success: function() {
                    console.log('todoitem.Insert() - request returned success');
                    // Write to the response and then send the notification in the background
                    request.respond();
                    push.gcm.send(item.channel, item.text, {
                        success: function(response) {
                            console.log('Push notification sent: ', response);
                        }, error: function(error) {
                            console.log('Error sending push notification: ', error);
                        }
                    });
                },
                error: function(response) {
                    console.log('todoitem.Insert() - error response');
                    console.log('Request failed with response code ' + response.status + '.' +
                        'Raw response: ' + response.text);
                   request.respond();
                }
            });
        }
        
        // iOS - 
        if (item.deviceToken) {        
            request.execute();
            // Set timeout to delay the notification, to provide time for the 
            // app to be closed on the device to demonstrate toast notifications
            setTimeout(function() {
                push.apns.send(item.deviceToken, {
                    alert: "Toast: " + item.text,
                    payload: {
                        inAppMessage: "Hey, a new item arrived: '" + item.text + "'"
                    }
                });
            }, 2500);
        }
        
    }    
}