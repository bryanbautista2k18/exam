import React, { memo } from "react";

interface InvalidFeedbackType {
    text?: string; 
}

const InvalidFeedback = memo((props: InvalidFeedbackType): React.JSX.Element => {
  return (
    <div className="invalid-feedback">{props.text}</div>
  );
});

export default InvalidFeedback;