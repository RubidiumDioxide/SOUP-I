import React from 'react';


const CompletionBar = ({ ratio }) => {
  const validRatio = Math.max(0, Math.min(1, ratio)); 

  return (
    <>
    <h6>Прогресс: </h6>
    <div className="completion-bar-container">
      <div className="completion-bar" style={{ width: `${validRatio * 100}%` }}></div>
    </div>
    </>
  );
};

export default CompletionBar;