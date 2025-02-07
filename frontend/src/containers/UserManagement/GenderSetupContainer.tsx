import React, { useState, useEffect, memo, useCallback, useMemo } from "react";
import { useLocation } from "react-router-dom";
import { toast } from "react-toastify";
import apiService from "../../services/apiService.ts";
import { InputConfigGenderType, InputConfigGenderErrorType } from "types/user-management/gender-setup.ts";
// import "../../../../css/admin.css";
import GenderSetupForm from "../../components/UserManagement/GenderSetupForm.tsx";
import Alert from "../../components/shared/Alert.tsx";
import CardHeader from "../../components/shared/CardHeader.tsx";
import SearchBar from "../../components/shared/SearchBar.tsx";
import TableLimiter from "../../components/shared/TableLimiter.tsx";
import TableHeader from "../../components/shared/TableHeader.tsx";
import TableBody from "../../components/shared/TableBody.tsx";
import TablePaginator from "../../components/shared/TablePaginator.tsx";

const GenderSetupContainer = memo((): React.JSX.Element => {
  const [show, setShow] = useState<{ [key: string]: boolean }>({ card: false, form: false });
  const [loading, setLoading] = useState<boolean>(false);
  const [message, setMessage] = useState<{ [key: string]: string }>({});
  const [input, setInput] = useState<InputConfigGenderType>({});
  const [inputError, setInputError] = useState<InputConfigGenderErrorType>({});
  const [disabled, setDisabled] = useState<boolean>(false);
  const tableHeaders: { [key: string]: string }[] = useMemo(() => {
    return [
      { text: "Title", className: "" },
      { text: "Description", className: "" },
      { text: "Status", className: "text-center" }
    ];
  }, []);
  const tableColumns: { [key: string]: unknown } = useMemo(() => {
    return {
      "title": null,
      "description": null
    };
  }, []);
  
  const [tableData, setTableData] = useState<{ [key: string]: unknown }[]>([]);
  const [searchKey, setSearchKey] = useState<string>("");
  const [currentPage, setCurrentPage] = useState<number>(1);
  const [hasMorePages, setHasMorePages] = useState<boolean>(false);
  const [pageLimit, setPageLimit] = useState<number>(10);
  const location = useLocation();

  useEffect(() => {
    getTableData();
  }, [searchKey, currentPage, pageLimit]);

  /**
   * Get table data.
   * @return Promise<void>
   */
  const getTableData = useCallback(async (): Promise<void> => {
    if (!loading) {
      try {
        setLoading(true);
        await apiService.get(`${location.pathname}?search_key=${searchKey}&page=${currentPage}&limit=${pageLimit}`)
          .then(({ data }) => {
            populateTableData(data.data.configGenders);
          })
          .catch((error) => {
            setShow({ card: false, form: false });
            const statusCode: number = error.status;
            if (statusCode !== 401) {
              toast.error(statusCode === 500 ? error.response.statusText : error.message);
            }
          });
      } catch (error) {
        setShow({ card: false, form: false });
        toast.error("Something went wrong! Please report this to the developer.");
      } finally {
        setLoading(false);
      }
    }
  }, [loading, searchKey, currentPage, pageLimit]);

  const searchTableData = useCallback((e: React.ChangeEvent): void => {
    setSearchKey((e.target as HTMLInputElement).value);
    setCurrentPage(1);
  }, []);

  const changePageLimit = useCallback((limit: number): void => {
    setPageLimit(limit);
    setCurrentPage(1);
  }, []);

  const populateTableData = useCallback((configGenders: { [key: string]: unknown }[]): void => {
    setShow({ card: true, form: false });
    setTableData(configGenders);
    setHasMorePages(configGenders.length === pageLimit);
  }, [pageLimit]);

  /**
   * Get previous page.
   * @return void
   */
  const getPrevPage = useCallback((): void => {
    if (!loading && currentPage > 1) {
      setCurrentPage((cp) => cp - 1);
    }
  }, [loading, currentPage]);

  /**
   * Get next page.
   * @return void
   */
  const getNextPage = useCallback((): void => {
    if (!loading && hasMorePages) {
      setCurrentPage((cp) => cp + 1);
    }
  }, [loading, hasMorePages]);

  /**
   * Handle change event.
   * @param e React.ChangeEvent
   * @return void
   */
  const handleChangeEvent = useCallback((e: React.ChangeEvent): void => {
    const currentField = (e.target as (HTMLInputElement|HTMLTextAreaElement));
    const value: string = currentField.value;
    const message: string = (value === "" ? currentField.title : "");

    setInput((i) => { return { ...i, [currentField.name]: value }; });
    setInputError((ie) => { return { ...ie, [`${currentField.name}Error`]: message }; });
  }, []);

  /**
   * Handle submit event.
   * @param e React.FormEvent<HTMLFormElement>
   * @return Promise<void>
   */
  const handleSubmitEvent = useCallback(async (e: React.FormEvent<HTMLFormElement>): Promise<void> => {
    e.preventDefault();
   
    const form = (e.target as HTMLFormElement);

    if (form.checkValidity()) {
      if (!loading) {
        try { 
          setLoading(true);
          setMessage({});
         
          if (input.id) {
            await apiService.put(`${location.pathname}/${input.id}?search_key=${searchKey}&page=${currentPage}&limit=${pageLimit}`, input)
              .then(({ data }) => {
                populateTableData(data.data.configGenders);
                setInput({});
                toast.success(data.message.text);
              })
              .catch((error) => {
                const statusCode: number = error.status;
                if (statusCode === 422) {
                  if (error.response.data?.errors?.length) {
                    for (const innerError of error.response.data.errors) {
                      setInputError((ie) => { return { ...ie, [`${innerError.field}Error`]: innerError.text } });
                    }
                  } else {
                    for (const innerError of error.response.data) {
                      const field = (innerError.propertyName[0].toLowerCase() + innerError.propertyName.slice(1));
                      setInputError((ie) => { return { ...ie, [`${field}Error`]: innerError.errorMessage } });
                    }
                  }
                } else if (statusCode !== 401) {
                  setMessage({ 
                    text: error?.response?.data?.message?.text ?? ([400, 404, 500].includes(statusCode) ? error.response.statusText : error.message),
                    type: error?.response?.data?.message?.type ?? "danger"
                  });
                }
              });
          } else {
            await apiService.post(`${location.pathname}?search_key=${searchKey}&page=${currentPage}&limit=${pageLimit}`, input)
              .then(({ data }) => {
                populateTableData(data.data.configGenders);
                setInput({});
                toast.success(data.message.text);
              })
              .catch((error) => {
                const statusCode: number = error.status;
                if (statusCode === 422) {
                  if (error.response.data?.errors?.length) {
                    for (const innerError of error.response.data.errors) {
                      setInputError((ie) => { return { ...ie, [`${innerError.field}Error`]: innerError.text } });
                    }
                  } else {
                    for (const innerError of error.response.data) {
                      const field = (innerError.propertyName[0].toLowerCase() + innerError.propertyName.slice(1));
                      setInputError((ie) => { return { ...ie, [`${field}Error`]: innerError.errorMessage } });
                    }
                  }
                } else if (statusCode !== 401) {
                  setMessage({ 
                    text: error?.response?.data?.message?.text ?? ([400, 404, 500].includes(statusCode) ? error.response.statusText : error.message),
                    type: error?.response?.data?.message?.type ?? "danger"
                  });
                }
              });
          }
        } catch (error) {
          setMessage({ 
            text: "Something went wrong! Please report this to the developer.", 
            type: "danger" 
          });
        } finally {
          setLoading(false);
          form.classList.remove("was-validated");
        }
      }
    } else {
      form.classList.add("was-validated");
    }
  }, [loading, input]);

  const closeForm = useCallback((): void => {
    setShow((s) => { return { ...s, form: false }; });
    setMessage({});
    setInput({});
    setInputError({});
    setDisabled(false);
  }, []);

  /**
   * Get data.
   * @param Object params 
   * @return Promise<void>
   */
  const getData = useCallback(async (params: { [key: string]: unknown }): Promise<void> => {
    if (!loading) {
      try {
        setLoading(true);

        await apiService.get(`${location.pathname}/${params.id}`)
          .then(({ data }) => {
            setInput({
              id: data.data.configGender.id,
              title: data.data.configGender.title,
              description: data.data.configGender.description
            });
            setDisabled(!params.editable as boolean);
            setShow((s) => { return { ...s, form: true }; });
          })
          .catch((error) => {
            const statusCode: number = error.status;
            if (statusCode !== 401) {
              toast.error(error?.response?.data?.message?.text ?? ([404, 500].includes(statusCode) ? error.response.statusText : error.message));
            }
          });
      } catch (error) {
        toast.error("Something went wrong! Please report this to the developer.");
      } finally {
        setLoading(false);
      }
    }
  }, [loading]);

  /**
   * Deactivate / Activate data.
   * @param Object params 
   * @return Promise<void>
   */
  const deActivateData = useCallback(async (params: { [key: string]: unknown }): Promise<void> => {
    if (!loading) {
      try {
        setLoading(true);

        await apiService.delete(`${location.pathname}/${params.id}?search_key=${searchKey}&page=${currentPage}&limit=${pageLimit}`)
          .then(({ data }) => {
            if (params.active) {
              toast.info(data.message.text);
            } else {
              toast.success(data.message.text);
            }
            populateTableData(data.data.configGenders);
          })
          .catch((error) => {
            const statusCode: number = error.status;
            if (statusCode !== 401) {
              toast.error(error?.response?.data?.message?.text ?? ([404, 500].includes(statusCode) ? error.response.statusText : error.message));
            }
          });
      } catch (error) {
        toast.error("Something went wrong! Please report this to the developer.");
      } finally {
        setLoading(false);
      }
    }
  }, [loading]);

  return (
    show.card
    ? (
        <>
          {message.text && <Alert type={`alert-${message.type}`} text={message.text} />}
          <div className="card">
            <CardHeader name="Gender" pluralizedName="Genders" show={show} setShow={setShow} />
            <div className="card-body">
              {show.form 
                ? (
                    <GenderSetupForm 
                      input={input}
                      inputError={inputError}
                      disabled={disabled}
                      handleChangeEvent={handleChangeEvent}
                      handleSubmitEvent={handleSubmitEvent}
                      closeForm={closeForm}
                    />
                  )
                : (
                    <>
                      <div className="d-flex justify-content-between align-items-center mb-3">
                        <SearchBar 
                          searchKey={searchKey}
                          searchTableData={searchTableData}
                        />
                        <TableLimiter 
                          pageLimit={pageLimit} 
                          changePageLimit={changePageLimit} 
                        />
                      </div>
                      <div className="table-responsive">
                        <table className="table table-sm table-striped gender-table">
                          <TableHeader tableHeaders={tableHeaders} />
                          <TableBody 
                            loading={loading}
                            tableColumns={tableColumns} 
                            tableData={tableData} 
                            getData={getData} 
                            deActivateData={deActivateData} 
                          />
                        </table>
                      </div>
                      <TablePaginator 
                        loading={loading} 
                        currentPage={currentPage} 
                        hasMorePages={hasMorePages}  
                        getPrevPage={getPrevPage} 
                        getNextPage={getNextPage} 
                      />
                    </>
                  )
              }
            </div>
            {/* /.card-body */}
          </div>
          {/* /.card */}
        </>
      )
    : (<></>)
  );
});

export default GenderSetupContainer;