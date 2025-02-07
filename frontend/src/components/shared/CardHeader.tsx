import React, { memo } from "react";

interface CardHeaderType 
{
    name: string;
    pluralizedName: string;
    show: { [key: string]: boolean };
    setShow: React.Dispatch<React.SetStateAction<{ [key: string]: boolean }>>;
}

const CardHeader = memo((props: CardHeaderType): React.JSX.Element => {
  return (
    <div className={`card-header ${props.show.form ? `text-center` : ``}`}>
        {props.show.form 
        ? (<div className="fw-bold m-0">{props.name} Form</div>)
        : (<div className="row align-items-center">
                <div className="col-6">
                    <strong>List of {props.pluralizedName}</strong>
                </div>
                <div className="col-6 text-end">
                    <button
                        className="btn btn-sm btn-dark"
                        onClick={() => props.setShow((s) => { return { ...s, form: true }; })}
                        type="button"
                    ><i className="bi bi-plus-lg"></i><span>New</span></button>
                </div>
            </div>
            )
        }
    </div>
  );
});

export default CardHeader;