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
        <Link to="/welcome" className="nav-link">Вход и регистрация</Link>
        <Link to="/allprojects" className="nav-link">Все проекты</Link>
        <Link to={`/user`} className="nav-link">Мой профиль</Link>
        <Link to="/myprojects" className="nav-link">Мои проекты</Link>
        <Link to="/mytasks" className="nav-link">Мои задачи</Link>
        <Link to="/notifications" className="nav-link">Уведомления</Link>
      </nav>
    </header>
  )
}; 