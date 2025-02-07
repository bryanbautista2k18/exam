import React, { memo } from "react";

interface SelectType {
  autoComplete?: string;
  className?: string;
  disabled?: boolean;
  id?: string;
  name?: string;
  onChange?: React.ChangeEventHandler<HTMLSelectElement>;
  required?: boolean; 
  title?: string;
  value?: string;
  data?: { [key: string]: unknown }[];
  default_value?: string;
  disabled_default?: boolean;
}

const Select = memo((props: SelectType): React.JSX.Element => {
  return (
    <select
      autoComplete={props.autoComplete}
      className={props.className}
      disabled={props.disabled}
      id={props.id}
      name={props.name}
      onChange={props.onChange}
      required={props.required}
      title={props.title}
      value={props.value}
    >{props.default_value && <option value="" disabled={props.disabled_default === false ? false : true}>{props.default_value}</option>}
      {props.data?.map((item: { [key: string]: unknown }, i) => (
        <option key={i} value={item.value as string}>{item.text as string}</option>
      ))}
    </select>
  );
});

export default Select;