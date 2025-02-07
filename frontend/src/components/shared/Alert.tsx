import React, { memo } from "react";

interface AlertType {
    type: string;
    text: string;
}

const Alert = memo((props: AlertType): React.JSX.Element => {
  return (
    <div className={`alert ${props.type}`} role="alert">{props.text}</div>
  );
});

export default Alert;