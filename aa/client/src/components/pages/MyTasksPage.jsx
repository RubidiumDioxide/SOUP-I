import React from 'react';

import TasksTable from '../tasks_comps/TasksTable'; 


export default function MyTasksPage(){
    return(
        <div>
            <TasksTable
              type="byassignee"
            />
        </div>
    )
}