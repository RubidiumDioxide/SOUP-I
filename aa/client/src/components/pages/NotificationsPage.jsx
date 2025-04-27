import React from 'react';
import Notifications from '../notification_comps/NotificationsTable'; 

export default function NotificationsPage(){
    return(
        <div>
            <Notifications
                userId={sessionStorage.getItem('savedUserID')}
            />
        </div>
    )
}