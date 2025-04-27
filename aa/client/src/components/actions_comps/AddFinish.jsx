import {React, useEffect, useState} from 'react'

export default function AddFinish({projectId, taskId, onAction, type}) { 
    const userId = Number(sessionStorage.getItem('savedUserID'));   
    const [addForm, setAddForm] = useState({
        description : '', 
        commit : ''
    }); 

    useEffect(() => {
        /*fetch(`/api/Users/${sessionStorage.getItem('savedUserID')}`)
            .then(response => response.json())
            .then(data => setUser(data))

        fetch(`/api/Teams/ForDisplay/Project/${projectId}`)
            .then(response => response.json())
            .then(data => setTeammates(data));*/ 
    }, [])

    function handleFormChange(e){
        setAddForm({
          ...addForm, 
          [e.target.name]: e.target.value
        })
      }    

    function handleAddForm(e) {
        e.preventDefault();

        console.log({
            id : 0, 
            projectId : projectId, 
            actorId : userId, 
            taskId : taskId, 
            description : addForm.description, 
            commit : addForm.commit, 
            date : Date.now
        })

       fetch(`/api/Actions`, {
            method : "POST", 
            headers : {
                "Content-Type" : "application/json"
            },             
            body : JSON.stringify({
                id : 0, 
                projectId : projectId, 
                actorId : userId, 
                taskId : taskId, 
                description : addForm.description, 
                commit : addForm.commit, 
                date : Date.now
            })
        })
            .then(response => {
                if(response.ok){
                    alert(`Действие успешно добавлено`); 
                    onAction(); 
                }
                else{
                    alert("Ошибка при добавлении действия. Перепроверьте введенные данные")
                }
            })

        if(type == "finish"){
            fetch(`/api/Tasks/Complete/${taskId}`, {
                method : "PUT", 
                headers : {
                    "Content-Type" : "application/json"
                } 
            })
                .then(onAction)
        }
    }

    return (
        <div>
            <form onSubmit={handleAddForm}>
                <input class='rounded-input' type="text" name="description" placeholder="комментарий" value={addForm.description} onChange={handleFormChange}/>
                <input class='rounded-input' type="text" name="commit" placeholder="gtihub коммит" value={addForm.commit} onChange={handleFormChange}/>
                
                <div class='app-div'>
                    <button class='rounded-button' type="submit">{(type == 'add')?'Добавить действие':'Добавить действие и завершить задачу'}</button>
                </div>
            </form>
        </div>
    )
}