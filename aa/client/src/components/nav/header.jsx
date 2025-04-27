import { useState } from 'react';
import { Link } from 'react-router-dom' ;


export default function Header(){
  const [current, setCurrent] = useState('h');
  const onClick = (e) => {
    console.log('click ', e);
    setCurrent(e.key);
  };

  return (
    <header>
      <nav>
        <Link to="/welcome" class="nav-link">Вход и регистрация</Link>
        <Link to="/allprojects" class="nav-link">Все проекты</Link>
        <Link to={`/user`} class="nav-link">Мой профиль</Link>
        <Link to="/myprojects" class="nav-link">Мои проекты</Link>
        <Link to="/mytasks" class="nav-link">Мои задачи</Link>
        <Link to="/notifications" class="nav-link">Уведомления</Link>
      </nav>
    </header>
  )
}; 