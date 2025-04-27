import React, { useEffect, useState } from "react";


const uri = '/api/users'; 

export default function SignUp({changeAppState}) { 
    const [signUpForm, setSignUpForm] = useState({
        name : "",
        password : "",  
      })

    function handleChange(e){
        setSignUpForm({
          ...signUpForm, 
          [e.target.name]: e.target.value
        })
    }

    function handleSignUpForm(e){
        e.preventDefault(); 
        
        fetch(uri, {
            method: "POST", 
            headers: {
                "Content-Type" : "application/json" 
            }, 
            body: JSON.stringify(signUpForm), 
        })
        .then(response => {
            if(response.ok){
                fetch(uri + `/Find/${signUpForm.name}`, {
                    method: "GET", 
                    headers: {
                        "Content-Type" : "application/json" 
                    } 
                })
                    .then(response =>{
                        if(response.ok){
                            response.json().then(user =>{
                                changeAppState("registered_user", user.id); 
                                alert(`Приятно познакомиться, ${user.name}`);
                            })
                        }
                        else{
                            changeAppState("observer", null);
                            alert("Не получилось зарегистрироваться:( Скорее всего, такой ник уже занят.");
                        }
                    })      
            } 
            else{
                changeAppState("observer", null);
                alert("Не получилось зарегистрироваться( Скорее всего, такой ник уже занят.");
            }

        })
    }

    return (
        <div className="app-div">
            <h4>Регистрация</h4>
            <form onSubmit={handleSignUpForm} >
                <div class='app-div'>
                    <input class='rounded-input' name="name" type="text" required minLength="1" maxLength="20" placeholder="имя пользователя" onChange={handleChange}/>
                    <input class='rounded-input' name="password" type="password" required minLength="4" maxLength="20" placeholder="очень надежный пароль" onChange={handleChange}/>
                </div>

                <div class='app-div'> 
                    <button class='rounded-button' type="submit">Зарегистрироваться</button>
                </div>
               
            </form>
        </div>
    );
}
