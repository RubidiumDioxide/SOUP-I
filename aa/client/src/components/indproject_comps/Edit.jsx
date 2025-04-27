import React, { useState } from 'react'
import convertDateFormat from '../../Extensions/convertDateFormat'; 


const uri = '/api/projects';

export default function Edit({project, onAction}) {
  const [editForm, setEditForm] = useState({
    name : project.name, 
    description : project.description,   
    dateDeadline : convertDateFormat(project.dateDeadline), 
    isPrivate : project.isPrivate 
  })

    function handleEditChange(e){
      setEditForm({
        ...editForm, 
        [e.target.name]: e.target.value
      })
    }

    function handleEditForm(e) {
        e.preventDefault();
 
      /*console.log({
        id : project.id, 
        name : editForm.name, 
        description : editForm.description, 
        creator : project.creator, 
        isComplete : project.isComplete,  
        dateBegan : project.dateBegan, 
        dateFinished : project.dateFinished,  
        dateDeadline : editForm.dateDeadline, 
        isPrivate : editForm.isPrivate 
      })*/
        
        fetch(uri + `/${project.id}`, {
            method: "PUT",
            headers: {
                "Content-Type" : "application/json"
            },
            body: JSON.stringify({
              id : project.id, 
              name : editForm.name, 
              description : editForm.description, 
              creator : project.creator, 
              isComplete : Boolean(project.isComplete),  
              dateBegan : project.dateBegan, 
              dateFinished : project.dateFinished,  
              dateDeadline : editForm.dateDeadline, 
              isPrivate : Boolean(editForm.isPrivate) 
            }),
        })
            .then(onAction)
    }

    return (
        <div>
            <h4>Edit project name & description</h4>
            
            <form onSubmit={handleEditForm}>
                <input class='rounded-input' type="text" name="name" value={editForm.name} onChange={handleEditChange}/>
                <input class='rounded-input' type="text" name="description" value={editForm.description} onChange={handleEditChange}/>
                <input class='rounded-input' type="date" name="dateDeadline" value={editForm.dateDeadline} onChange={handleEditChange}/>
                Private
                <input class='rounded-input' type="checkbox" name="isPrivate" value={editForm.isPrivate} onChange={handleEditChange}/>

                <button class='rounded-button' type="submit">Submit Changes</button>
            </form>
        </div>
    )
} 