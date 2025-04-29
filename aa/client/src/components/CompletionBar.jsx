import React from 'react';


const CompletionBar = ({ ratio }) => {
  return (
    (ratio == -1)? 
      null
      :
      <>
      <h6>Прогресс: </h6>
        <div className="completion-bar-container">
          <div className="completion-bar" style={{ width: `${ratio * 100}%` }}></div>
        </div> 
      </>
  );
};

export default CompletionBar;