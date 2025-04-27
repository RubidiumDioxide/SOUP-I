import React, { useState } from 'react'

export default function Request({project, senderId}) {
  const [requestForm, setRequestForm] = useState({
    senderId : senderId, 
    role : "" 
  })

    function handleRequestChange(e){
      setRequestForm({
        ...requestForm, 
        [e.target.name]: e.target.value
      })
    }

    function handleRequestForm(e) { 
      e.preventDefault();

      fetch(`/api/Notifications`, {
        method : "POST", 
        headers : {
            "Content-Type" : "application/json"
        },             
        body : JSON.stringify({
            id : 0, 
            projectId : project.id, 
            senderId : senderId, 
            receiverId : project.creatorId, 
            role : requestForm.role,
            type : "request" 
        })
        })
    }

    return (
        <div>
            <form onSubmit={handleRequestForm}>
                <select class='rounded-select' value={requestForm.role} name="role" onChange={handleRequestChange}>
                    <option value="">Select Role</option>
                    <option value='Руководитель отдела дизайна'>Руководитель отдела дизайна</option>
                    <option value='Веб-дизайнер'>Веб-дизайнер</option>
                    <option value='Графический дизайнер'>Графический дизайнер</option>
                    <option value='3D-дизайнер'>3D-дизайнер</option>
                    <option value='UI/UX дизайнер'>UI/UX дизайнер</option>
                    <option value='Руководитель отдела разработки'>Руководитель отдела разработки</option>
                    <option value='Архитектор'>Архитектор</option>
                    <option value='Fullstack-разработчик'>Fullstack-разработчик</option>
                    <option value='Front end-разработчик'>Front end-разработчик</option>
                    <option value='Back end-разработчик'>Back end-разработчик</option>
                    <option value='Разработчик баз данных'>Разработчик баз данных</option>
                    <option value='Руководитель отдела внедрения и тестирования'>Руководитель отдела внедрения и тестирования</option>
                    <option value='DevOps-инженер'>DevOps-инженер</option>
                    <option value='Тестировщик'>Тестировщик</option>
                    <option value='Руководитель отдела информационной безопасности'>Руководитель отдела информационной безопасности</option>
                    <option value='Специалист по безопасности'>Специалист по безопасности</option>
                    <option value='Системный администратор'>Системный администратор</option>
                    <option value='Руководитель отдела аналитики'>Руководитель отдела аналитики</option>
                    <option value='Бизнес-аналитик'>Бизнес-аналитик</option>
                    <option value='Системный аналитик'>Системный аналитик</option>
                </select>

                <button class='rounded-button' type="submit">Send request to participate</button>
            </form>
        </div>
    )
} 