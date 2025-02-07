import React, { memo } from "react";
import "./GenderSetup.css";
import GenderSetupContainer from "./../../../containers/UserManagement/GenderSetupContainer.tsx";

const GenderSetup = memo((): React.JSX.Element => {
  return (
    <div className="container">
        <div className="row justify-content-center">
          <div className="col-12">
            <GenderSetupContainer />
        </div>
      </div>
    </div>
  );
});

export default GenderSetup;