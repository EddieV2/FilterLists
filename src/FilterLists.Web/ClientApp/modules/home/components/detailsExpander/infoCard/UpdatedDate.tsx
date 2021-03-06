import * as React from "react";
import * as moment from "moment";

interface IProps {
    discontinuedDate: string;
    updatedDate: string;
}

export const UpdatedDate = (props: IProps) => {
    return props.discontinuedDate
               ? null
               : props.updatedDate
                 ? <li className="d-md-none list-group-item">
                       <p>Updated: {moment(props.updatedDate).isValid()
                                        ? moment(props.updatedDate).format("l")
                                        : "N/A"}</p>
                   </li>
                 : null;
};