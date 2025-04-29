import {React, useEffect, useState} from 'react';

import TasksTable from '../tasks_comps/TasksTable'; 
import CompletionBar from '../CompletionBar';

const userID = sessionStorage.getItem("savedUserID"); 


export default function MyTasksPage(){
    const [ratio, setRatio] = useState(0); 


    useEffect(() => {
        //set Ratio
        fetch(`/api/tasks/ratio/byuser/${userID}`)
            .then(response => {
                if (!response.ok) {
                    console.error('Error fetching ratio');
                    return null; 
                }
                return response.json();
            })
            .then(data => {
                if (data !== null) {
                    setRatio(data); 
                }
            })
            .catch(error => {
                console.error('Fetch error:', error);
            });
    }, [])


    return(
        <div className="app-div">
            {/* как будто оно здесь не надо на самом-то деле <CompletionBar ratio={ratio}/>*/}
            <TasksTable
              type="byassignee"
            />
        </div>
    )
}