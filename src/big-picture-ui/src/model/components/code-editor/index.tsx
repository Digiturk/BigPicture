import * as React from 'react';
import * as WFace from '@wface/components';
import MonacoEditor from 'react-monaco-editor';
import Node from '../../base/node';
import IOC, { IHttpService } from '@wface/ioc';
import CodeBlock from '../../base/code-block';
import { OptionsObject } from 'notistack';
import ReactResizeDetector from 'react-resize-detector';

export interface CodeEditorProps extends WFace.BaseScreenProps {
  node?: Node;
}

interface CodeEditorState {
  codeBlock: CodeBlock;
  isLoading: boolean;
}

export class CodeEditor extends React.Component<CodeEditorProps, CodeEditorState> {
  editor: any;

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

  editorDidMount = (editor, monaco) => {
    this.editor = editor;
    editor.focus();
  };


  public render() {
    const options = {
      selectOnLineNumbers: true,
      readOnly: true
    };

    return (
      <WFace.WCard>
        <WFace.WCardHeader title={this.props.node.Name} />
        <WFace.WCardContent style={{ padding: 0 }}>
          <div style={{ position: 'relative' }}>
            <WFace.WPaper style={{ height: '82vh', width: '100%'}}>
              <ReactResizeDetector
                handleWidth
                handleHeight
                onResize={() => { this.editor && this.editor.layout(); }}
              >
                <MonacoEditor
                  width="100%"
                  height="100%"
                  language="javascript"
                  theme="vs-dark"
                  value={this.state.codeBlock ? this.state.codeBlock.Code : ""}
                  options={options}              
                  editorDidMount={this.editorDidMount}
                />
              </ReactResizeDetector>
            </WFace.WPaper>
            {this.state.isLoading &&
              <div style={{ position: 'absolute', top: 0, left: 0, width: '100%', height: '100%', backgroundColor: '#FFFFFF66', display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
                <WFace.WCircularProgress style={{ color: 'white' }} size={60} />
              </div>
            }
          </div>
        </WFace.WCardContent>
      </WFace.WCard>
    );
  }
}