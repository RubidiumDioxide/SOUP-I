import React, { useEffect, useState } from "react";


const uri = '/api/users'; 

export default function Login({changeAppState}) { 
    const [logInForm, setLogInForm] = useState({
        name : "",
        password : "",  
      })

    function handleChange(e){
        setLogInForm({
          ...logInForm, 
          [e.target.name]: e.target.value
        })
    }


    function handleLogInForm(e){
        e.preventDefault(); 

        fetch(uri + `/Find/${logInForm.name}`, {
            method: "GET", 
            headers: {
                "Content-Type" : "application/json" 
            } 
        })
            .then(response =>{
                if(response.ok){
                    response.json()
                    .then(user =>{
                        changeAppState("registered_user", user.id); 
                        alert(`Давно не виделись, ${user.name}!`); 
                    })
                }
                else{
                    changeAppState("observer", null);   
                    alert("Не удалось войти в учетную запись :(. Попробуйте проверить имя и пароль")
                }
            })
    }

    return (
        <div className="app-div">
            <h4>Вход</h4>
            <form onSubmit={handleLogInForm}>
                <div className='app-div'>
                    <input className='rounded-input' name="name" type="text" required minLength="1" maxLength="20" placeholder="имя пользователя" onChange={handleChange}/>
                    <input className='rounded-input' name="password" type="password" required minLength="4" maxLength="20" placeholder="пароль" onChange={handleChange}/> 
                </div>

                <div className='app-div'> 
                    <button className='rounded-button' type="submit">Войти</button>
                </div>
            </form>
        </div>
    );
}
