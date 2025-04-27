import React from 'react';
import ProjectsTable from '../projects_comps/ProjectsTable'; 

export default function AllProjectsPage(){
    return(
        <div>
            <ProjectsTable 
                type={"all"} 
            />
        </div>
    )
}