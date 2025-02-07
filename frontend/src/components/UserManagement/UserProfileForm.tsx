import React, { memo } from "react";
import { InputUserType, InputUserErrorType } from "types/user-management/user-profile.ts";
import Input from "./../shared/Input.tsx";
import Select from "./../shared/Select.tsx";
import InvalidFeedback from "./../shared/InvalidFeedback.tsx";

interface UserProfileFormType {
  input: InputUserType;
  inputError: InputUserErrorType;
  configGenders: { [key: string]: unknown }[];
  disabled: boolean;
  handleChangeEvent: React.ChangeEventHandler<HTMLInputElement | HTMLSelectElement>;
  handleSubmitEvent: React.FormEventHandler<HTMLFormElement>;
  closeForm: () => void;
}

const UserProfileForm = memo((props: UserProfileFormType): React.JSX.Element => {
  return (
    <>
      <form 
        className="row justify-content-center"
        id="form"
        noValidate={true}
        onSubmit={props.handleSubmitEvent}
      ><div className="col-sm-12 col-md-6">
          <Input
            name="id"
            type="hidden"
            value={props.input.id ?? ""}  
          />
          {/* NAME */}
          <div className="mb-3">
            <label 
              className="fw-bold"
              htmlFor="nameInput"
            >Name <span className="required"></span></label>
            <Input
              autoComplete="off"
              autoFocus={true}
              className={`form-control form-control-sm ${props.inputError.nameError && `is-invalid`}`}
              disabled={props.disabled}
              id="nameInput"
              name="name"
              onChange={props.handleChangeEvent}
              placeholder="Enter the name"
              required={true}
              title="Name is required."
              type="text"
              value={props.input.name ?? ""}
            />
            <InvalidFeedback text={props.inputError.nameError ? props.inputError.nameError : "Name is required."} />
          </div>
          {/* EMAIL */}
          <div className="mb-3">
            <label 
              className="fw-bold"
              htmlFor="emailInput"
            >Email <span className="required"></span></label>
            <Input
              autoComplete="off"
              className={`form-control form-control-sm ${props.inputError.emailError && `is-invalid`}`}
              disabled={props.disabled}
              id="emailInput"
              name="email"
              onChange={props.handleChangeEvent}
              placeholder="Enter the email"
              required={true}
              title="Email is required."
              type="text"
              value={props.input.email ?? ""}
            />
            <InvalidFeedback text={props.inputError.emailError ? props.inputError.emailError : "Email is required."} />
          </div>
          {/* GENDER */}
          <div className="mb-3">
            <label 
              className="fw-bold"
              htmlFor="genderSelect"
            >Gender <span className="required"></span></label>
            <Select
              autoComplete="off"
              className={`form-control form-control-sm ${props.inputError.configGenderIdError && `is-invalid`}`}
              disabled={props.disabled}
              id="genderSelect"
              name="configGenderId"
              onChange={props.handleChangeEvent}
              required={true}
              title="Gender is required."
              value={props.input.configGenderId ?? ""}
              data={props.configGenders}
              default_value="Select a gender"
              disabled_default={true}
            />
            <InvalidFeedback text={props.inputError.configGenderIdError ? props.inputError.configGenderIdError : "Gender is required."} />
          </div>          
          {/* BIRTHDATE */}
          <div className="mb-3">
            <label 
              className="fw-bold"
              htmlFor="birthDateInput"
            >Birthdate <span className="required"></span></label>
            <Input
              autoComplete="off"
              className={`form-control form-control-sm ${props.inputError.birthDateError && `is-invalid`}`}
              disabled={props.disabled}
              id="birthDateInput"
              name="birthDate"
              onChange={props.handleChangeEvent}
              required={true}
              title="Birthdate is required."
              type="date"
              value={props.input.birthDate ?? ""}
            />
            <InvalidFeedback text={props.inputError.birthDateError ? props.inputError.birthDateError : "Birthdate is required."} />
          </div>
        </div>
      </form>
      <div className="row justify-content-center">
        <div className="col-sm-12 col-md-6">
          <div className="row">
            {/* Close Button */}
            <div className="col-6">
              <button
                className="btn btn-danger w-100"
                onClick={props.closeForm}
                type="button"
              ><i></i><span>Close</span></button>
            </div>
            {/* Save Button */}
            <div className="col-6">
              <button
                className="btn btn-primary w-100"
                disabled={props.disabled}
                form="form"
                type="submit"
              ><i></i><span>Save</span></button>
            </div>
          </div>
        </div>
      </div>  
    </>
  );
});

export default UserProfileForm;