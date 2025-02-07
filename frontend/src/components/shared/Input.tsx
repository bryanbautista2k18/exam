import React, { memo } from "react";

interface InputType {
  ariaDescribedby?: string;
  ariaLabel?: string;
  autoComplete?: string;
  autoFocus?: boolean;
  className?: string;
  disabled?: boolean;
  id?: string;
  name?: string;
  onChange?: React.ChangeEventHandler<HTMLInputElement>;
  placeholder?: string;
  readOnly?: boolean;
  required?: boolean;
  style?: { [key: string]: string };
  title?: string;
  type?: string;
  value?: string;
}

const Input = memo((props : InputType): React.JSX.Element => {
  return (
    <input 
      aria-describedby={props.ariaDescribedby}
      aria-label={props.ariaLabel}
      autoComplete={props.autoComplete}
      autoFocus={props.autoFocus}
      className={props.className}
      disabled={props.disabled}
      id={props.id}
      name={props.name}
      onChange={props.onChange}
      placeholder={props.placeholder}
      readOnly={props.readOnly}
      required={props.required}
      style={props.style}
      title={props.title}
      type={props.type}
      value={props.value}
    />
  );
});

export default Input;