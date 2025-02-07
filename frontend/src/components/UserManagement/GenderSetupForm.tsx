import React, { memo } from "react";
import { InputConfigGenderType, InputConfigGenderErrorType } from "types/user-management/gender-setup.ts";
import Input from "./../shared/Input.tsx";
import Textarea from "./../shared/Textarea.tsx";
import InvalidFeedback from "./../shared/InvalidFeedback.tsx";

interface GenderSetupFormType {
  input: InputConfigGenderType;
  inputError: InputConfigGenderErrorType;
  disabled: boolean;
  handleChangeEvent: React.ChangeEventHandler<HTMLInputElement | HTMLTextAreaElement>;
  handleSubmitEvent: React.FormEventHandler<HTMLFormElement>;
  closeForm: () => void;
}

const GenderSetupForm = memo((props: GenderSetupFormType): React.JSX.Element => {
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
          {/* TITLE */}
          <div className="mb-3">
            <label 
              className="fw-bold"
              htmlFor="titleInput"
            >Title <span className="required"></span></label>
            <Input
              autoComplete="off"
              autoFocus={true}
              className={`form-control form-control-sm ${props.inputError.titleError && `is-invalid`}`}
              disabled={props.disabled}
              id="titleInput"
              name="title"
              onChange={props.handleChangeEvent}
              placeholder="Enter the title"
              required={true}
              title="Title is required."
              type="text"
              value={props.input.title ?? ""}
            />
            <InvalidFeedback text={props.inputError.titleError ? props.inputError.titleError : "Title is required."} />
          </div>
          {/* DESCRIPTION */}
          <div className="mb-3">
            <label 
              className="fw-bold"
              htmlFor="descriptionTextArea"
            >Description</label>
            <Textarea
              autoComplete="off"
              className={`form-control form-control-sm ${props.inputError.descriptionError && `is-invalid`}`}
              cols={100}
              disabled={props.disabled}
              id="descriptionTextArea"
              name="description"
              onChange={props.handleChangeEvent}
              placeholder="Enter the description"
              rows={3}
              value={props.input.description ?? ""}
            />
            <InvalidFeedback text={props.inputError.descriptionError} />
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

export default GenderSetupForm;