import React, { useEffect, useState } from "react";
import SignUp from '../signup_comps/SignUp'; 
import LogIn from '../login_comps/LogIn'; 

const uri = '/api/users'; 

export default function Welcome({changeAppState}) { 
  const [welcomeState, setWelcomeState] = useState("none"); //possible values: none, login, signup  

    return (
        <div className="app-div">
          <h1 className="glow">Добро пожаловать в SOUP-I!</h1>  
          <p>
            <button className='rounded-button' onClick={() => {setWelcomeState("login")}}>
              Вход
            </button>
            <button className='rounded-button' onClick={() => {setWelcomeState("signup")}}>
              Регистрация
            </button>
          </p>
          
          {(welcomeState === "signup")? 
            <SignUp 
              changeAppState={changeAppState}
            />          
            :
            null
          }
          {(welcomeState === "login")? 
            <LogIn 
              changeAppState={changeAppState} 
            />          
            :
            null
          }
        </div>
    );
}
