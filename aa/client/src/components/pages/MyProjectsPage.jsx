import React from 'react';
import ProjectsTable from '../projects_comps/ProjectsTable'; 

export default function MyProjectsPage(){
    return(
        <div>
            <ProjectsTable
                type={"my"}
            />
        </div>
    )
}