import * as React from 'react';
import * as WFace from '@wface/components';
import MonacoEditor from 'react-monaco-editor';
import Node from '../../base/node';
import IOC, { IHttpService } from '@wface/ioc';
import CodeBlock from '../../base/code-block';
import { OptionsObject } from 'notistack';

export interface CodeEditorProps extends WFace.BaseScreenProps {
  node?: Node;
}

interface CodeEditorState {
  codeBlock: CodeBlock;
  isLoading: boolean;
}

export class CodeEditor extends React.Component<CodeEditorProps, CodeEditorState> {
  constructor(props: CodeEditorProps) {
    super(props);

    this.state = {
      codeBlock: null,
      isLoading: false,
    }
  }

  componentDidMount() {
    this.setState({ isLoading: true }, () => {
      this.props.httpService.get("code/" + this.props.node.Id)
        .then(codeBlock => this.setState({ codeBlock }))
        .catch(error => this.props.showSnackbar("An error occured when fetching code: " + error.Message, "error"))
        .finally(() => this.setState({ isLoading: false }));
    });
  }

  public render() {
    const options = {
      selectOnLineNumbers: true,
      readOnly: true
    };

    return (
      <WFace.WCard>
        <WFace.WCardHeader title={"sad"} />
        <WFace.WCardContent style={{ padding: 0 }}>
          <WFace.WPaper>
            <MonacoEditor
              width="100%"
              height="82vh"
              language="javascript"
              theme="vs-dark"
              value={this.state.codeBlock ? this.state.codeBlock.Code : "Loading"}
              options={options}            
            // onChange={this.onChange}
            // editorDidMount={this.editorDidMount}
            />
          </WFace.WPaper>
        </WFace.WCardContent>
      </WFace.WCard>
    );
  }
}