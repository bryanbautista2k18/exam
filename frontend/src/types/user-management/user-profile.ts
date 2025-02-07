export interface InputUserType {
    id?: string;
    name?: string;
    email?: string;
    configGenderId?: string;
    birthDate?: string;
}
  
export interface InputUserErrorType {
    nameError?: string;
    emailError?: string;
    configGenderIdError?: string;
    birthDateError?: string;
}