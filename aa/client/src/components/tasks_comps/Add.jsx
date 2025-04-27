import { Descriptions } from 'antd';
import {React, useEffect, useState} from 'react'

export default function Add({projectId, onAction}) {
    const [teammates, setTeammates] = useState(); 
    const userId = Number(sessionStorage.getItem('savedUserID'));
    const [addForm, setAddForm] = useState({
        name : "",
        description : "", 
        assigneeName : "" 
    }); 

    useEffect(() => {
        fetch(`/api/Teams/AssignableTeammates/ByUserProject/ForDisplay/${userId}/${projectId}`)
            .then(response => response.json())
            .then(data => setTeammates(data)); 
    }, [])

    function handleFormChange(e){
        setAddForm({
          ...addForm, 
          [e.target.name]: e.target.value
        })
      }    

    function handleAddForm(e) {
        e.preventDefault();

        console.log(addForm.assigneeName); 

        fetch(`/api/Tasks/${addForm.assigneeName}`, {
            method : "POST", 
            headers : {
                "Content-Type" : "application/json"
            },             
            body : JSON.stringify({
                id : 0, 
                projectId : projectId, 
                creatorId : userId, 
                assigneeId : 0, // no need to provide, controller method gets user on it's own 
                name : addForm.name,
                description : addForm.description, 
                isComplete : false
            })
        })
            .then(response => {
                if(response.ok){
                    alert(`Задача успешно добавлена`); 
                    onAction(); 
                }
                else{
                    alert("Ошибка при добавлении задачи. Перепроверьте введенные данные")
                }
            })
    }

    return (
        (teammates)?       
        <div>
            <form onSubmit={handleAddForm}>
                <input class='rounded-input' type="text" name="name" placeholder="название" value={addForm.name} onChange={handleFormChange}/>

                <input class='rounded-input' type="text" name="description" placeholder="описание" value={addForm.description} onChange={handleFormChange}/>

                <select class='rounded-select' value={addForm.assigneeName} name="assigneeName" onChange={handleFormChange}>
                    <option value="">Select Assignee</option> 
                    {teammates.map(teammate => 
                        <option key={teammate} value={teammate.toString()}>{teammate}</option>
                    )}    
                </select>
                
                <button class='rounded-button' type="submit">Add Task</button>
            </form>
        </div>
        :
        <p>{"Sorry, I messed up the loading :( text me @rubidiumoxide"}</p>    
    )
}