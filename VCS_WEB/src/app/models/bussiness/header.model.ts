import { BaseFilter } from "../base.model";

export class HeaderFilter extends BaseFilter {
    id?: string;
    stt?: number;
    vehicleCode: string = '';
    vehicleName: string = '';
    isCheckOut?: boolean;
    isVoice?: boolean;
    isPrint?: boolean;
    statusVehicle?: string;
    statusProcess?: string;
    TimeCheckOut?: Date;
    NoteOut?: string;
    NoteIn?: string;
    isActive?: boolean | string | null;
    fromDate?: Date;
    toDate?: Date;
    SortColumn: string = '';
    IsDescending: boolean = true;
}