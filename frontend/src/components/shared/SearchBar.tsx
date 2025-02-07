import React, { memo } from "react";
import Input from "./Input";

interface SearchBar {
    searchKey: string;
    searchTableData: React.ChangeEventHandler<HTMLInputElement>;
}

const SearchBar = memo((props: SearchBar): React.JSX.Element => {
  return (
    <div className="input-group w-25">
        <span className="input-group-text bi bi-search" id="search-key"></span>
        <Input 
            ariaDescribedby="search-key"
            ariaLabel="Search title" 
            autoComplete="off"
            className="form-control form-control-sm"
            onChange={props.searchTableData}
            placeholder="Search title"
            type="text"
            value={props.searchKey}
        />
    </div>
  );
});

export default SearchBar;