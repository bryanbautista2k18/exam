import React, { memo } from "react";

interface TextareaType {
    autoComplete?: string;
    className?: string;
    cols?: number;
    disabled?: boolean;
    id?: string;
    name?: string;
    onChange?: React.ChangeEventHandler<HTMLTextAreaElement>;
    placeholder?: string;
    required?: boolean;
    rows?: number;
    style?: { [key: string]: string };
    title?: string;
    value?: string;
}

const Textarea = memo((props: TextareaType): React.JSX.Element => {
  return (
    <textarea
      autoComplete={props.autoComplete}
      className={props.className}
      cols={props.cols}
      disabled={props.disabled}
      id={props.id}
      name={props.name}
      onChange={props.onChange}
      placeholder={props.placeholder}
      required={props.required}
      rows={props.rows}
      style={props.style}
      title={props.title}
      value={props.value}
    ></textarea>
  );
});

export default Textarea;