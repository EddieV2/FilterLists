import * as React from "react";
import { IMaintainerAdditionalListDto } from "../IFilterListDetailsDto";

interface IProps {
    name: string;
    additionalLists: IMaintainerAdditionalListDto[];
}

export const MaintainerAdditionalLists = (props: IProps) => {
    return <div className="col-9">
               {props.additionalLists && props.additionalLists.length > 0
                    ? <div>
                          <h4>More by {props.name}:</h4>
                          <ul>
                              {props.additionalLists.map(
                                  (l: IMaintainerAdditionalListDto, i: number) => <li key={i}>{l.name}</li>)}
                          </ul>

                      </div>
                    : null}
           </div>;
};